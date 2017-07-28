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

    public sealed class JsonObject : JsonElement
    {
        private readonly Dictionary<string, JsonElement> _objectMembers = new Dictionary<string, JsonElement>();

        public override JsonElementType Type => JsonElementType.Object;

        public override Dictionary<string, JsonElement> GetObjectMembers()
        {
            return _objectMembers;
        }

        internal override void ToJson(
            IJsonContext context, IJsonWriter writer, IJsonMapper<JsonElement> elMapper)
        {
            writer.WriteRaw(JsonLiterals.ObjectStart);
            if (_objectMembers.Count > 0)
            {
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
                    writer.WriteRaw(member.Key.EncodeToJsonString());
                    writer.WriteRaw(JsonLiterals.Colon);
                    elMapper.ToJson(context, writer, member.Value);
                }
            }
            writer.WriteRaw(JsonLiterals.ObjectEnd);
        }
    }
}