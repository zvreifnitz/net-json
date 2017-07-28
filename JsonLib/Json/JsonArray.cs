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
    using System.Collections.Generic;
    using Parser;

    public sealed class JsonArray : JsonElement
    {
        private readonly List<JsonElement> _elements = new List<JsonElement>();

        public override JsonElementType Type => JsonElementType.Array;

        public override List<JsonElement> GetArrayElements()
        {
            return _elements;
        }

        internal override void ToJson(
            IJsonContext context, IJsonWriter writer, IJsonMapper<JsonElement> elMapper)
        {
            writer.WriteRaw(JsonLiterals.ArrayStart);
            if (_elements.Count > 0)
            {
                elMapper.ToJson(context, writer, _elements[0]);
                for (var i = 1; i < _elements.Count; i++)
                {
                    writer.WriteRaw(JsonLiterals.Comma);
                    elMapper.ToJson(context, writer, _elements[i]);
                }
            }
            writer.WriteRaw(JsonLiterals.ArrayEnd);
        }
    }
}