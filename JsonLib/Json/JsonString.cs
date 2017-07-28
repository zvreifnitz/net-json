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

namespace com.github.zvreifnitz.JsonLib.Json
{
    using Parser;

    public sealed class JsonString : JsonElement
    {
        private readonly string _value;

        public JsonString(string value)
        {
            _value = value;
        }

        public override JsonElementType Type => JsonElementType.String;

        public override string GetStringValue()
        {
            return _value;
        }

        internal override void ToJson(
            IJsonContext context, IJsonWriter writer, IJsonMapper<JsonElement> elMapper)
        {
            if (_value == null)
            {
                writer.WriteRaw(JsonLiterals.Null);
            }
            else
            {
                writer.EncodeAndWrite(_value);
            }
        }
    }
}