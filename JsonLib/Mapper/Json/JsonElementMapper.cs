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

    internal sealed class JsonElementMapper : JsonElementMapperBase<JsonElement>
    {
        public override JsonElement FromJson(IJsonSerializators context, IJsonReader reader)
        {
            switch (reader.GetNextToken())
            {
                case JsonToken.ObjectStart:
                    reader.RepeatLastToken();
                    return JsonObjectMapper.FromJson(context, reader);
                case JsonToken.String:
                    return new JsonString(reader.ReadValue());
                case JsonToken.False:
                    return JsonBoolean.False;
                case JsonToken.True:
                    return JsonBoolean.True;
                case JsonToken.Null:
                    return JsonNull.Null;
                case JsonToken.Number:
                    return JsonNull.Null;
                case JsonToken.ArrayStart:
                    reader.RepeatLastToken();
                    return JsonArrayMapper.FromJson(context, reader);
                case JsonToken.ObjectEmpty:
                    return new JsonObject();
                case JsonToken.ArrayEmpty:
                    return new JsonArray();
                default:
                    return ExceptionHelper.ThrowInvalidJsonException<JsonElement>();
            }
        }
    }
}