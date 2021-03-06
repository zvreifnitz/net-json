﻿/*
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

namespace com.github.zvreifnitz.JsonLib.Mapper.Collection
{
    using System;
    using System.Collections.Generic;
    using Common;

    internal sealed class ArrayMapperBuilder : DelegatingMapperBuilderBase
    {
        private static readonly Type ListType = typeof(List<>);

        protected override bool IsRequestedTypeSupported(Type type)
        {
            return type.IsArray;
        }

        protected override Type GenerateDelegatingType(Type type)
        {
            return ListType.MakeGenericType(type.GetElementType());
        }

        protected override Type[] GetMethodTypes(Type type, Type delegatingType)
        {
            return new[] {type.GetElementType()};
        }

        private IJsonMapper<T[]> BuildReflectionInvoke<T>(IJsonSerializator<List<T>> serializator)
        {
            return new ArrayMapper<T>(serializator);
        }

        private sealed class ArrayMapper<T> : IJsonMapper<T[]>
        {
            private readonly IJsonMapper<List<T>> _mapper;

            public ArrayMapper(IJsonSerializator<List<T>> serializator)
            {
                _mapper = serializator.Mapper;
            }

            public bool CanSerialize => true;

            public bool CanDeserialize => true;

            public void Init(IJsonContext context)
            {
            }

            public void ToJson(IJsonContext context, IJsonWriter writer, T[] instance)
            {
                _mapper.ToJson(context, writer, instance == null ? null : new List<T>(instance));
            }

            public T[] FromJson(IJsonContext context, IJsonReader reader)
            {
                var result = _mapper.FromJson(context, reader);
                return result?.ToArray();
            }
        }
    }
}