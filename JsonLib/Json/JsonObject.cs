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

    public sealed class JsonObject : JsonElement
    {
        private readonly Dictionary<string, JsonElement> _objectMembers = new Dictionary<string, JsonElement>();

        public override JsonType Type => JsonType.Object;

        public override Dictionary<string, JsonElement> GetObjectMembers()
        {
            return _objectMembers;
        }

        internal override void ToJson(IJsonSerializators context, IJsonWriter writer)
        {
            writer.WriteRaw(JsonLiterals.ObjectStart);
            if (_objectMembers.Count > 0)
            {
                var keySerializator = context.GetJsonSerializator<string>();
                var valueSerializator = context.GetJsonSerializator<JsonElement>();
                var addComma = false;
                foreach (var member in _objectMembers)
                {
                    if (addComma)
                    {
                        writer.WriteRaw(JsonLiterals.Comma);
                    }
                    else
                    {
                        addComma = true;
                    }
                    keySerializator.Mapper.ToJson(context, writer, member.Key);
                    writer.WriteRaw(JsonLiterals.Colon);
                    valueSerializator.Mapper.ToJson(context, writer, member.Value);
                }
            }
            writer.WriteRaw(JsonLiterals.ObjectEnd);
        }

        internal new static JsonObject FromJson(IJsonSerializators context, IJsonReader reader)
        {
            var token = reader.GetNextToken();
            if (token == JsonToken.Null)
            {
                return null;
            }
            if (token != JsonToken.ObjectStart)
            {
                return ExceptionHelper.ThrowInvalidJsonException<JsonObject>();
            }
            var keySerializator = context.GetJsonSerializator<string>();
            var valueSerializator = context.GetJsonSerializator<JsonElement>();
            var result = new JsonObject();
            while (true)
            {
                token = reader.GetNextToken();
                if (token == JsonToken.ObjectEnd)
                {
                    break;
                }
                if (token != JsonToken.Comma)
                {
                    reader.RepeatLastToken();
                }
                var key = keySerializator.Mapper.FromJson(context, reader);
                JsonSerializatorsHelper.ThrowIfNotMatch(reader, JsonToken.Colon);
                var value = valueSerializator.Mapper.FromJson(context, reader);
                result._objectMembers.Add(key, value);
            }
            return result;
        }
    }
}