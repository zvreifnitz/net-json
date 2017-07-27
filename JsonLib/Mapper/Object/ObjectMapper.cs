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

namespace com.github.zvreifnitz.JsonLib.Mapper.Object
{
    using System;
    using System.Collections.Generic;
    using Helper;
    using Parser;
    using JsonLib.Json;

    internal sealed class ObjectMapper<TClass> : IJsonMapper<TClass>
    {
        private readonly Func<TClass> _instanceProvider;
        private readonly Dictionary<string, IJsonGetterSetter<TClass>> _getterSetters;

        internal ObjectMapper(Func<TClass> instanceProvider,
            Dictionary<string, IJsonGetterSetter<TClass>> getterSetters)
        {
            _instanceProvider = instanceProvider;
            _getterSetters = getterSetters;
        }

        public bool CanSerialize => true;

        public bool CanDeserialize => true;

        public void Init(IJsonSerializators context)
        {
            foreach (var getterSetter in _getterSetters.Values)
            {
                getterSetter.Init(context);
            }
        }

        public void ToJson(IJsonSerializators context, IJsonWriter writer, TClass instance)
        {
            if (instance == null)
            {
                writer.WriteRaw(JsonLiterals.Null);
            }
            else
            {
                writer.WriteRaw(JsonLiterals.ObjectStart);
                if (_getterSetters.Count > 0)
                {
                    var addComma = false;
                    foreach (var member in _getterSetters)
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
                        member.Value.ToJson(context, writer, instance);
                    }
                }
                writer.WriteRaw(JsonLiterals.ObjectEnd);
            }
        }

        public TClass FromJson(IJsonSerializators context, IJsonReader reader)
        {
            var token = reader.GetNextToken();
            if (token == JsonToken.Null)
            {
                return default(TClass);
            }
            if (token != JsonToken.ObjectStart)
            {
                return ExceptionHelper.ThrowInvalidJsonException<TClass>();
            }
            var result = _instanceProvider();
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
                JsonSerializatorsHelper.ThrowIfNotMatch(reader, JsonToken.String);
                var key = reader.ReadValue();
                JsonSerializatorsHelper.ThrowIfNotMatch(reader, JsonToken.Colon);
                if (_getterSetters.TryGetValue(key, out IJsonGetterSetter<TClass> getterSetter))
                {
                    getterSetter.FromJson(context, reader, result);
                }
            }
            return result;
        }
    }
}