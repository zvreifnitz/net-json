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

namespace com.github.zvreifnitz.JsonLib.Mapper.Collection
{
    using System;
    using System.Reflection;
    using System.Collections.Generic;
    using Helper;
    using Parser;
    using Common;

    internal abstract class SetMapperBuilderBase : MapperBuilderBase
    {
        protected abstract Type GenericTypeDefinition { get; }

        protected sealed override bool IsRequestedTypeSupported(Type type)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == GenericTypeDefinition;
        }

        protected sealed class SetMapper<TSetImpl, TSet, TItem> : IJsonMapper<TSet>
            where TSet : ISet<TItem>
            where TSetImpl : TSet, new()
        {
            private readonly IJsonMapper<TItem> _mapper;

            public SetMapper(IJsonSerializator<TItem> serializator)
            {
                _mapper = serializator.Mapper;
            }

            public bool CanSerialize => true;

            public bool CanDeserialize => true;

            public void Init(IJsonSerializators context)
            {
            }

            public void ToJson(IJsonSerializators context, IJsonWriter writer, TSet instance)
            {
                if (instance == null)
                {
                    writer.WriteRaw(JsonLiterals.Null);
                }
                else
                {
                    writer.WriteRaw(JsonLiterals.ArrayStart);
                    if (instance.Count > 0)
                    {
                        bool addComma = false;
                        foreach (var item in instance)
                        {
                            if (addComma)
                            {
                                writer.WriteRaw(JsonLiterals.Comma);
                            }
                            else
                            {
                                addComma = true;
                            }
                            ToJsonItem(context, writer, item);
                        }
                    }
                    writer.WriteRaw(JsonLiterals.ArrayEnd);
                }
            }

            private void ToJsonItem(IJsonSerializators context, IJsonWriter writer, TItem instance)
            {
                if (_mapper.CanSerialize)
                {
                    _mapper.ToJson(context, writer, instance);
                }
                else
                {
                    writer.WriteRaw(JsonLiterals.Null);
                }
            }

            public TSet FromJson(IJsonSerializators context, IJsonReader reader)
            {
                JsonToken token = reader.GetNextToken();
                if (token == JsonToken.Null)
                {
                    return default(TSet);
                }
                if (token == JsonToken.ArrayEmpty)
                {
                    return new TSetImpl();
                }
                if (token != JsonToken.ArrayStart)
                {
                    return ExceptionHelper.ThrowInvalidJsonException<TSet>();
                }
                TSetImpl result = new TSetImpl();
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
                    result.Add(FromJsonItem(context, reader));
                }
                return result;
            }

            private TItem FromJsonItem(IJsonSerializators context, IJsonReader reader)
            {
                return _mapper.CanDeserialize
                    ? _mapper.FromJson(context, reader)
                    : default(TItem);
            }
        }
    }

    internal sealed class ItfSetMapperBuilder : SetMapperBuilderBase
    {
        protected override Type GenericTypeDefinition => typeof(ISet<>);

        protected override Type[] GetMethodTypes(Type type, Type[] types)
        {
            return types;
        }

        private IJsonMapper<ISet<T>> BuildReflectionInvoke<T>(IJsonSerializator<T> serializator)
        {
            return new SetMapper<HashSet<T>, ISet<T>, T>(serializator);
        }
    }

    internal sealed class HashSetMapperBuilder : SetMapperBuilderBase
    {
        protected override Type GenericTypeDefinition => typeof(HashSet<>);

        protected override Type[] GetMethodTypes(Type type, Type[] types)
        {
            return types;
        }

        private IJsonMapper<HashSet<T>> BuildReflectionInvoke<T>(IJsonSerializator<T> serializator)
        {
            return new SetMapper<HashSet<T>, HashSet<T>, T>(serializator);
        }
    }

    internal sealed class SortedSetMapperBuilder : SetMapperBuilderBase
    {
        protected override Type GenericTypeDefinition => typeof(SortedSet<>);

        protected override Type[] GetMethodTypes(Type type, Type[] types)
        {
            return types;
        }

        private IJsonMapper<SortedSet<T>> BuildReflectionInvoke<T>(IJsonSerializator<T> serializator)
        {
            return new SetMapper<SortedSet<T>, SortedSet<T>, T>(serializator);
        }
    }
}