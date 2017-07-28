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

namespace com.github.zvreifnitz.JsonLib.Mapper.Json
{
    using JsonLib.Json;
    using Helper;

    internal sealed class JsonArrayMapper : JsonElementMapperBase<JsonArray>
    {
        public override JsonArray FromJson(IJsonContext context, IJsonReader reader)
        {
            var token = reader.GetNextToken();
            if (token == JsonToken.Null)
            {
                return null;
            }
            if (token == JsonToken.ArrayEmpty)
            {
                return new JsonArray();
            }
            if (token != JsonToken.ArrayStart)
            {
                return ExceptionHelper.ThrowInvalidJsonException<JsonArray>();
            }
            var result = new JsonArray();
            var elements = result.GetArrayElements();
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
                var value = JsonElementMapper.FromJson(context, reader);
                elements.Add(value);
            }
            return result;
        }
    }
}