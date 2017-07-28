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
    using System;
    using System.IO;

    internal sealed class JsonWriter : IJsonWriter, IDisposable
    {
        private readonly TextWriter _writer;

        public JsonWriter(TextWriter writer)
        {
            _writer = writer;
        }

        public void EncodeAndWrite(string value)
        {
            if (value == null)
            {
                _writer.Write(JsonLiterals.Null);
                return;
            }

            _writer.Write(JsonLiterals.QuotationMark);
            foreach (var ch in value)
            {
                switch (ch)
                {
                    case JsonLiterals.QuotationMark:
                    {
                        _writer.Write(JsonLiterals.QuotationMarkJson);
                        break;
                    }
                    case JsonLiterals.ReverseSolidus:
                    {
                        _writer.Write(JsonLiterals.ReverseSolidusJson);
                        break;
                    }
                    case JsonLiterals.Solidus:
                    {
                        _writer.Write(JsonLiterals.SolidusJson);
                        break;
                    }
                    case JsonLiterals.Backspace:
                    {
                        _writer.Write(JsonLiterals.BackspaceJson);
                        break;
                    }
                    case JsonLiterals.Formfeed:
                    {
                        _writer.Write(JsonLiterals.FormfeedJson);
                        break;
                    }
                    case JsonLiterals.Newline:
                    {
                        _writer.Write(JsonLiterals.NewlineJson);
                        break;
                    }
                    case JsonLiterals.CarriageReturn:
                    {
                        _writer.Write(JsonLiterals.CarriageReturnJson);
                        break;
                    }
                    case JsonLiterals.HorizontalTab:
                    {
                        _writer.Write(JsonLiterals.HorizontalTabJson);
                        break;
                    }
                    default:
                    {
                        _writer.Write(ch);
                        break;
                    }
                }
            }
            _writer.Write(JsonLiterals.QuotationMark);
        }

        public void WriteRaw(string value)
        {
            _writer.Write(value);
        }

        public void WriteRaw(char value)
        {
            _writer.Write(value);
        }

        public void Dispose()
        {
        }
    }
}