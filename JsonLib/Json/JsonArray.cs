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
    using Helper;
    using Parser;
    
    public sealed class JsonArray : JsonElement
    {
        private readonly List<JsonElement> _elements = new List<JsonElement>();

        public override JsonType Type => JsonType.Array;

        public override List<JsonElement> GetArrayElements()
        {
            return _elements;
        }

        internal override void ToJson(IJsonSerializators context, IJsonWriter writer)
        {
            writer.WriteRaw(JsonLiterals.ArrayStart);
            if (_elements.Count > 0)
            {
                var serializator = context.GetJsonSerializator<JsonElement>();
                serializator.Mapper.ToJson(context, writer, _elements[0]);
                for (int i = 1; i < _elements.Count; i++)
                {
                    writer.WriteRaw(JsonLiterals.Comma);
                    serializator.Mapper.ToJson(context, writer, _elements[i]);
                }
            }
            writer.WriteRaw(JsonLiterals.ArrayEnd);
        }

        internal new static JsonArray FromJson(IJsonSerializators context, IJsonReader reader)
        {
            var token = reader.GetNextToken();
            if (token == JsonToken.Null)
            {
                return null;
            }
            if (token != JsonToken.ArrayStart)
            {
                return ExceptionHelper.ThrowInvalidJsonException<JsonArray>();
            }
            var serializator = context.GetJsonSerializator<JsonElement>();
            var result = new JsonArray();
            while (true)
            {
                token = reader.GetNextToken();
                if (token == JsonToken.ArrayEnd)
                {
                    break;
                }
                if (token != JsonToken.Comma)
                {
                    reader.RepeatLastToken();
                }
                var value = serializator.Mapper.FromJson(context, reader);
                result._elements.Add(value);
            }
            return result;
        }
    }
}