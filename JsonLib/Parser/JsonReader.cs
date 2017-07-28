/*
 * (C) Copyright 2017 zvreifnitz
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

namespace com.github.zvreifnitz.JsonLib.Parser
{
    using Helper;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Globalization;

    internal sealed class JsonReader : IJsonReader, IDisposable
    {
        private readonly TextReader _reader;
        private readonly Stack<Mode> _modeStack;
        private readonly StringBuilder _intermediateValue;

        private JsonToken? _expToken;
        private JsonToken _currToken;
        private char _currChar;
        private string _currValue;
        private bool _moveToNextToken = true;
        private bool _moveToNextChar = true;

        public JsonReader(TextReader reader)
        {
            _reader = reader;
            _intermediateValue = new StringBuilder(127);
            _modeStack = new Stack<Mode>(31);
            _modeStack.Push(Mode.Unspecified);
        }

        public JsonToken GetNextToken()
        {
            return _currToken = MoveToNextToken() ? ReadNextToken() : _currToken;
        }

        public void RepeatLastToken()
        {
            _moveToNextToken = false;
        }

        public string ReadValue()
        {
            return _currValue;
        }

        public void Dispose()
        {
        }

        private char GetNextChar()
        {
            return _currChar = MoveToNextChar() ? ReadNextChar() : _currChar;
        }

        private char GetNextCharSkipWhitespace()
        {
            char currChar;
            do
            {
                currChar = GetNextChar();
            } while (char.IsWhiteSpace(currChar));
            return currChar;
        }

        private char? PeekNextChar()
        {
            var nextCharInt = _reader.Peek();
            return nextCharInt == -1
                ? (char?)null
                : (char)nextCharInt;
        }

        public void RepeatLastChar()
        {
            _moveToNextChar = false;
        }

        private char ReadNextChar()
        {
            var nextCharInt = _reader.Read();
            return nextCharInt == -1
                ? ExceptionHelper.ThrowEndOfStreamException<char>()
                : (char)nextCharInt;
        }

        private JsonToken ReadNextToken()
        {
            return ReadExpectedToken() ?? ReadUnexpectedToken();
        }

        private JsonToken? ReadExpectedToken()
        {
            if (_expToken == null)
            {
                return null;
            }
            var token = _expToken.Value;
            _expToken = null;
            switch (token)
            {
                case JsonToken.String:
                    return ReadString(JsonToken.Colon);
                case JsonToken.Colon:
                    return GetNextCharSkipWhitespace() == JsonLiterals.Colon
                        ? JsonToken.Colon
                        : ExceptionHelper.ThrowInvalidJsonException<JsonToken>();
                case JsonToken.Comma:
                    return GetNextCharSkipWhitespace() == JsonLiterals.Comma
                        ? JsonToken.Comma
                        : ExceptionHelper.ThrowInvalidJsonException<JsonToken>();
                default:
                    return null;
            }
        }

        private JsonToken ReadUnexpectedToken()
        {
            switch (CurrentMode)
            {
                case Mode.Object:
                    return ReadUnexpectedToken_Object();
                case Mode.Array:
                    return ReadUnexpectedToken_Array();
                default:
                    return ReadUnexpectedToken_Unspecified();
            }
        }

        private JsonToken ReadUnexpectedToken_Object()
        {
            var currChar = GetNextCharSkipWhitespace();
            if (currChar == JsonLiterals.Comma)
            {
                _expToken = JsonToken.String;
                return JsonToken.Comma;
            }
            if (currChar == JsonLiterals.ObjectEnd)
            {
                _modeStack.Pop();
                return JsonToken.ObjectEnd;
            }
            RepeatLastChar();
            return ReadUnexpectedToken_Unspecified();
        }

        private JsonToken ReadUnexpectedToken_Array()
        {
            var currChar = GetNextCharSkipWhitespace();
            if (currChar == JsonLiterals.Comma)
            {
                return JsonToken.Comma;
            }
            if (currChar == JsonLiterals.ArrayEnd)
            {
                _modeStack.Pop();
                return JsonToken.ArrayEnd;
            }
            RepeatLastChar();
            return ReadUnexpectedToken_Unspecified();
        }

        private JsonToken ReadUnexpectedToken_Unspecified()
        {
            while (true)
            {
                var currChar = GetNextCharSkipWhitespace();
                if (currChar == JsonLiterals.QuotationMark)
                {
                    RepeatLastChar();
                    return ReadString(null);
                }
                if (currChar == JsonLiterals.ObjectStart)
                {
                    if (GetNextCharSkipWhitespace() == JsonLiterals.ObjectEnd)
                    {
                        return JsonToken.ObjectEmpty;
                    }
                    _modeStack.Push(Mode.Object);
                    RepeatLastChar();
                    _expToken = JsonToken.String;
                    return JsonToken.ObjectStart;
                }
                if (char.IsDigit(currChar) || currChar == '-')
                {
                    RepeatLastChar();
                    return ReadNumber();
                }
                if (currChar == 't' || currChar == 'f')
                {
                    RepeatLastChar();
                    return ReadBoolean();
                }
                if (currChar == 'n')
                {
                    RepeatLastChar();
                    return ReadNull();
                }
                if (currChar == JsonLiterals.ArrayStart)
                {
                    if (GetNextCharSkipWhitespace() == JsonLiterals.ArrayEnd)
                    {
                        return JsonToken.ArrayEmpty;
                    }
                    _modeStack.Push(Mode.Array);
                    RepeatLastChar();
                    return JsonToken.ArrayStart;
                }
                return ExceptionHelper.ThrowInvalidJsonException<JsonToken>();
            }
        }

        private JsonToken ReadString(JsonToken? expToken)
        {
            try
            {
                var currChar = GetNextCharSkipWhitespace();
                if (currChar != JsonLiterals.QuotationMark)
                {
                    return ExceptionHelper.ThrowInvalidJsonException<JsonToken>();
                }
                _expToken = expToken;

                while (true)
                {
                    currChar = GetNextChar();
                    if (currChar == JsonLiterals.ReverseSolidus)
                    {
                        currChar = GetNextChar();
                        switch (currChar)
                        {
                            case JsonLiterals.QuotationMark:
                                _intermediateValue.Append(JsonLiterals.QuotationMark);
                                break;
                            case JsonLiterals.ReverseSolidus:
                                _intermediateValue.Append(JsonLiterals.ReverseSolidus);
                                break;
                            case JsonLiterals.Solidus:
                                _intermediateValue.Append(JsonLiterals.Solidus);
                                break;
                            case 'b':
                                _intermediateValue.Append(JsonLiterals.Backspace);
                                break;
                            case 'f':
                                _intermediateValue.Append(JsonLiterals.Formfeed);
                                break;
                            case 'n':
                                _intermediateValue.Append(JsonLiterals.Newline);
                                break;
                            case 'r':
                                _intermediateValue.Append(JsonLiterals.CarriageReturn);
                                break;
                            case 't':
                                _intermediateValue.Append(JsonLiterals.HorizontalTab);
                                break;
                            case 'u':
                                string tmp = "";
                                for (int i = 0; i < 4; i++)
                                {
                                    tmp += GetNextChar();
                                }
                                _intermediateValue.Append(UnicodeEscapedStringToChar(tmp));
                                break;
                            default:
                                return ExceptionHelper.ThrowInvalidJsonException<JsonToken>();
                        }
                    }
                    else if (currChar == JsonLiterals.QuotationMark)
                    {
                        break;
                    }
                    else
                    {
                        _intermediateValue.Append(currChar);
                    }
                }
                _currValue = _intermediateValue.ToString();
                return JsonToken.String;
            }
            finally
            {
                _intermediateValue.Clear();
            }
        }

        private JsonToken ReadNumber()
        {
            try
            {
                var currChar = GetNextChar();
                if (char.IsDigit(currChar) || currChar == '-')
                {
                    _intermediateValue.Append(currChar);
                }
                else
                {
                    return ExceptionHelper.ThrowInvalidJsonException<JsonToken>();
                }

                while (true)
                {
                    var peekChar = PeekNextChar();
                    if (peekChar == null)
                    {
                        _currValue = _intermediateValue.ToString();
                        return JsonToken.Number;
                    }
                    else
                    {
                        currChar = peekChar.Value;
                        if (char.IsDigit(currChar) || currChar == '.' ||
                            currChar == '-' || currChar == '+' ||
                            currChar == 'e' || currChar == 'E')
                        {
                            _intermediateValue.Append(GetNextChar());
                            continue;
                        }
                        _currValue = _intermediateValue.ToString();
                        return JsonToken.Number;
                    }
                }
            }
            finally
            {
                _intermediateValue.Clear();
            }
        }

        private JsonToken ReadBoolean()
        {
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    _intermediateValue.Append(GetNextChar());
                }
                if (JsonLiterals.True == _intermediateValue.ToString())
                {
                    return JsonToken.True;
                }
                _intermediateValue.Append(GetNextChar());
                return JsonLiterals.False == _intermediateValue.ToString()
                    ? JsonToken.False
                    : ExceptionHelper.ThrowInvalidJsonException<JsonToken>();
            }
            finally
            {
                _intermediateValue.Clear();
            }
        }

        private JsonToken ReadNull()
        {
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    _intermediateValue.Append(GetNextChar());
                }
                return JsonLiterals.Null == _intermediateValue.ToString()
                    ? JsonToken.Null
                    : ExceptionHelper.ThrowInvalidJsonException<JsonToken>();
            }
            finally
            {
                _intermediateValue.Clear();
            }
        }

        private bool MoveToNextToken()
        {
            if (_moveToNextToken)
            {
                return true;
            }
            _moveToNextToken = true;
            return false;
        }

        private bool MoveToNextChar()
        {
            if (_moveToNextChar)
            {
                return true;
            }
            _moveToNextChar = true;
            return false;
        }

        private Mode CurrentMode => _modeStack.Peek();

        private static char UnicodeEscapedStringToChar(string input)
        {
            if (int.TryParse(input, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int result))
            {
                return (char)result;
            }
            return ExceptionHelper.ThrowInvalidJsonException<char>();
        }

        private enum Mode
        {
            Unspecified,
            Object,
            Array
        }
    }
}