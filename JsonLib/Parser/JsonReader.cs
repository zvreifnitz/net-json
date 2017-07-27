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

    internal sealed class JsonReader : IJsonReader, IDisposable
    {
        private readonly TextReader _reader;
        private readonly Stack<Mode> _modeStack;
        private readonly StringBuilder _intermediateValue;
        private char _prevChar;
        private char _currChar;
        private JsonToken _currToken;
        private string _currentValue;
        private bool _moveToNextToken = true;
        private bool _moveToNextChar = true;

        public JsonReader(TextReader reader)
        {
            _reader = reader;
            _intermediateValue = new StringBuilder(127);
            _modeStack = new Stack<Mode>(127);
            _modeStack.Push(Mode.Unspecified);
        }

        public JsonToken GetNextToken()
        {
            while (true)
            {
                if (MoveToNextToken)
                {
                    if (MoveToNextChar)
                    {
                        var nextCharTmp = ReadNextChar();
                        if (nextCharTmp == null)
                        {
                            if (CurrentMode == Mode.Number)
                            {
                                var tmp = ProcessChar_NumberMode(true);
                                if (tmp == null)
                                {
                                    return ExceptionHelper.ThrowEndOfStreamException<JsonToken>();
                                }
                                _currToken = tmp.Value;
                                return _currToken;
                            }
                            return ExceptionHelper.ThrowEndOfStreamException<JsonToken>();
                        }
                        _prevChar = _currChar;
                        _currChar = nextCharTmp.Value;
                    }
                    var result = ProcessChar();
                    if (result != null)
                    {
                        _currToken = result.Value;
                        return _currToken;
                    }
                }
                else
                {
                    return _currToken;
                }
            }
        }

        public void RepeatLastToken()
        {
            _moveToNextToken = false;
        }

        private void RepeatLastChar()
        {
            _moveToNextChar = false;
        }

        private char? ReadNextChar()
        {
            var nextCharInt = _reader.Read();
            return ((nextCharInt == -1) ? (char?)null : (char)nextCharInt);
        }

        public void Dispose()
        {
        }

        private JsonToken? ProcessChar()
        {
            switch (CurrentMode)
            {
                case Mode.String: return ProcessChar_StringMode();
                case Mode.Boolean: return ProcessChar_BooleanMode();
                case Mode.Null: return ProcessChar_NullMode();
                case Mode.Number: return ProcessChar_NumberMode(false);
                case Mode.Object: return ProcessChar_ObjectMode();
                case Mode.Array: return ProcessChar_ArrayMode();
                default: return ProcessChar_UnspecifiedMode();
            }
        }

        private JsonToken? ProcessChar_ArrayMode()
        {
            _currentValue = null;
            if (_currChar == ',')
            {
                return JsonToken.Comma;
            }
            if (_currChar == ']')
            {
                _modeStack.Pop();
                return JsonToken.ArrayEnd;
            }
            return ProcessChar_Default();
        }

        private JsonToken? ProcessChar_ObjectMode()
        {
            _currentValue = null;
            if (_currChar == ',')
            {
                return JsonToken.Comma;
            }
            if (_currChar == ':')
            {
                return JsonToken.Colon;
            }
            if (_currChar == '}')
            {
                _modeStack.Pop();
                return JsonToken.ObjectEnd;
            }
            return ProcessChar_Default();
        }

        private JsonToken? ProcessChar_NumberMode(bool forceEnd)
        {
            var currentChar = forceEnd ? '!' : _currChar;
            if (char.IsDigit(currentChar) || currentChar == '.' ||
                currentChar == '-' || currentChar == '+' ||
                currentChar == 'e' || currentChar == 'E')
            {
                _intermediateValue.Append(_currChar);
                return null;
            }
            _currentValue = _intermediateValue.ToString();
            _intermediateValue.Clear();
            _modeStack.Pop();
            RepeatLastChar();
            return JsonToken.Number;
        }

        private JsonToken? ProcessChar_NullMode()
        {
            _currentValue = null;
            _intermediateValue.Append(_currChar);
            if (_intermediateValue.Length < 4)
            {
                return null;
            }
            if (_intermediateValue.ToString() != JsonLiterals.Null)
            {
                return ExceptionHelper.ThrowInvalidJsonException<JsonToken>();
            }
            _intermediateValue.Clear();
            _modeStack.Pop();
            return JsonToken.Null;
        }

        private JsonToken? ProcessChar_BooleanMode()
        {
            _currentValue = null;
            _intermediateValue.Append(_currChar);
            switch (_intermediateValue.Length)
            {
                case 4:
                    if (_intermediateValue.ToString() == JsonLiterals.True)
                    {
                        _intermediateValue.Clear();
                        _modeStack.Pop();
                        return JsonToken.True;
                    }
                    else
                    {
                        return null;
                    }
                case 5:
                    if (_intermediateValue.ToString() == JsonLiterals.False)
                    {
                        _intermediateValue.Clear();
                        _modeStack.Pop();
                        return JsonToken.False;
                    }
                    else
                    {
                        return ExceptionHelper.ThrowInvalidJsonException<JsonToken>();
                    }
                default: return null;
            }
        }

        private JsonToken? ProcessChar_StringMode()
        {
            _intermediateValue.Append(_currChar);
            if (_currChar != JsonHelper.QuotationMark || _prevChar == JsonHelper.ReverseSolidus)
            {
                return null;
            }
            _currentValue = _intermediateValue.ToString().DecodeFromJsonString();
            _intermediateValue.Clear();
            _modeStack.Pop();
            return JsonToken.String;
        }

        private JsonToken? ProcessChar_UnspecifiedMode()
        {
            _currentValue = null;
            return ProcessChar_Default();
        }

        private JsonToken? ProcessChar_Default()
        {
            if (char.IsWhiteSpace(_currChar))
            {
                return null;
            }
            if (_currChar == '"')
            {
                _modeStack.Push(Mode.String);
                _intermediateValue.Append(_currChar);
                return null;
            }
            if (_currChar == '{')
            {
                _modeStack.Push(Mode.Object);
                return JsonToken.ObjectStart;
            }
            if (char.IsDigit(_currChar) || _currChar == '-')
            {
                _modeStack.Push(Mode.Number);
                _intermediateValue.Append(_currChar);
                return null;
            }
            if (_currChar == 't'||_currChar == 'f')
            {
                _modeStack.Push(Mode.Boolean);
                _intermediateValue.Append(_currChar);
                return null;
            }
            if (_currChar == 'n')
            {
                _modeStack.Push(Mode.Null);
                _intermediateValue.Append(_currChar);
                return null;
            }
            if (_currChar == '[')
            {
                _modeStack.Push(Mode.Array);
                return JsonToken.ArrayStart;
            }
            return ExceptionHelper.ThrowInvalidJsonException<JsonToken>();
        }

        private bool MoveToNextToken
        {
            get
            {
                var result = _moveToNextToken;
                _moveToNextToken = true;
                return result;
            }
        }

        private bool MoveToNextChar
        {
            get
            {
                var result = _moveToNextChar;
                _moveToNextChar = true;
                return result;
            }
        }

        private Mode CurrentMode => _modeStack.Peek();

        public string ReadValue()
        {
            return _currentValue;
        }

        private enum Mode
        {
            Unspecified,
            String,
            Number,
            Object,
            Array,
            Boolean,
            Null
        }
    }
}