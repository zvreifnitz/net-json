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

    internal sealed class NullableMapperBuilder : MapperBuilderBase
    {
        private static readonly Type NullableType = typeof(Nullable<>);

        protected override bool IsRequestedTypeSupported(Type type)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == NullableType;
        }

        protected override Type[] GetMethodTypes(Type type, Type[] types)
        {
            return types;
        }

        protected override bool CheckGenericTypeArgs(Type[] types)
        {
            return types[0].GetTypeInfo().IsValueType;
        }

        private IJsonMapper<T?> BuildReflectionInvoke<T>(IJsonSerializator<T> serializator) where T : struct
        {
            return new NullableJsonMapper<T>(serializator);
        }

        private sealed class NullableJsonMapper<T> : IJsonMapper<T?> where T : struct
        {
            private readonly IJsonMapper<T> _mapper;

            public NullableJsonMapper(IJsonSerializator<T> serializator)
            {
                _mapper = serializator.Mapper;
            }

            public bool CanSerialize => true;

            public bool CanDeserialize => true;

            public void Init(IJsonSerializators context)
            {
            }

            public void ToJson(IJsonSerializators context, IJsonWriter writer, T? instance)
            {
                if (instance == null)
                {
                    writer.WriteRaw(JsonLiterals.Null);
                }
                else
                {
                    _mapper.ToJson(context, writer, instance.Value);
                }
            }

            public T? FromJson(IJsonSerializators context, IJsonReader reader)
            {
                var token = reader.GetNextToken();
                if (token == JsonToken.Null)
                {
                    return null;
                }
                reader.RepeatLastToken();
                return _mapper.FromJson(context, reader);
            }
        }
    }
}