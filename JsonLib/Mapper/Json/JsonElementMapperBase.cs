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
    using Parser;

    internal abstract class JsonElementMapperBase<T> : IJsonMapper<T> where T : JsonElement
    {
        protected IJsonMapper<JsonElement> JsonElementMapper;
        protected IJsonMapper<JsonArray> JsonArrayMapper;
        protected IJsonMapper<JsonObject> JsonObjectMapper;

        public bool CanSerialize => true;

        public bool CanDeserialize => true;

        public void Init(IJsonContext context)
        {
            JsonElementMapper = context.GetSerializator<JsonElement>().Mapper;
            JsonArrayMapper = context.GetSerializator<JsonArray>().Mapper;
            JsonObjectMapper = context.GetSerializator<JsonObject>().Mapper;
        }

        public void ToJson(IJsonContext context, IJsonWriter writer, T instance)
        {
            if (instance == null)
            {
                writer.WriteRaw(JsonLiterals.Null);
            }
            else
            {
                instance.ToJson(context, writer, JsonElementMapper);
            }
        }

        public abstract T FromJson(IJsonContext context, IJsonReader reader);
    }
}