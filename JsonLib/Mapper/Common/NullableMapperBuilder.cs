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

namespace com.github.zvreifnitz.JsonLib.Mapper.Common
{
    using System;
    using System.Reflection;
    using Parser;
    
    internal sealed class NullableMapperBuilder : JsonMapperBuilderBase
    {
        protected override Type GenericTypeDefinition => typeof(Nullable<>);

        protected override Type[] GetMethodTypes(Type type, Type[] types)
        {
            return types;
        }

        protected override bool CheckTypes(Type[] types)
        {
            return types[0].GetTypeInfo().IsValueType;
        }

        private IJsonMapper<T?> BuildReflectionInvoke<T>(IJsonSerializator<T> serializator) where T : struct
        {
            return new NullableJsonMapper<T>(serializator);
        }

        private sealed class NullableJsonMapper<T> : IJsonMapper<T?> where T : struct
        {
            private readonly IJsonSerializator<T> _serializator;

            public NullableJsonMapper(IJsonSerializator<T> serializator)
            {
                _serializator = serializator;
            }

            public bool CanSerialize => true;

            public bool CanDeserialize => true;

            public void ToJson(IJsonSerializators context, IJsonWriter writer, T? instance)
            {
                if (instance == null)
                {
                    writer.WriteRaw(JsonLiterals.Null);
                }
                else
                {
                    _serializator.Mapper.ToJson(context, writer, instance.Value);
                }
            }

            public T? FromJson(IJsonSerializators context, IJsonReader reader)
            {
                JsonToken token = reader.GetNextToken();
                if (token == JsonToken.Null)
                {
                    return null;
                }
                reader.RepeatLastToken();
                return _serializator.Mapper.FromJson(context, reader);
            }
        }
    }
}