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
    using System.Collections.ObjectModel;
    using Helper;
    using Parser;
    using Common;

    internal abstract class ListMapperBuilderBase : MapperBuilderBase
    {
        protected abstract Type GenericTypeDefinition { get; }

        protected sealed override bool IsRequestedTypeSupported(Type type)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == GenericTypeDefinition;
        }

        protected sealed class ListMapper<TListImpl, TList, TItem> : IJsonMapper<TList>
            where TList : IList<TItem>
            where TListImpl : TList, new()
        {
            private readonly IJsonMapper<TItem> _mapper;

            public ListMapper(IJsonSerializator<TItem> serializator)
            {
                _mapper = serializator.Mapper;
            }

            public bool CanSerialize => true;

            public bool CanDeserialize => true;

            public void Init(IJsonSerializators context)
            {
            }

            public void ToJson(IJsonSerializators context, IJsonWriter writer, TList instance)
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
                        ToJsonItem(context, writer, instance[0]);
                        for (int i = 1; i < instance.Count; i++)
                        {
                            writer.WriteRaw(JsonLiterals.Comma);
                            ToJsonItem(context, writer, instance[i]);
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

            public TList FromJson(IJsonSerializators context, IJsonReader reader)
            {
                JsonToken token = reader.GetNextToken();
                if (token == JsonToken.Null)
                {
                    return default(TList);
                }
                if (token != JsonToken.ArrayStart)
                {
                    return ExceptionHelper.ThrowInvalidJsonException<TList>();
                }
                TListImpl result = new TListImpl();
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

    internal sealed class ItfListMapperBuilder : ListMapperBuilderBase
    {
        protected override Type GenericTypeDefinition => typeof(IList<>);

        protected override Type[] GetMethodTypes(Type type, Type[] types)
        {
            return types;
        }

        private IJsonMapper<IList<T>> BuildReflectionInvoke<T>(IJsonSerializator<T> serializator)
        {
            return new ListMapper<List<T>, IList<T>, T>(serializator);
        }
    }

    internal sealed class ListMapperBuilder : ListMapperBuilderBase
    {
        protected override Type GenericTypeDefinition => typeof(List<>);

        protected override Type[] GetMethodTypes(Type type, Type[] types)
        {
            return types;
        }

        private IJsonMapper<List<T>> BuildReflectionInvoke<T>(IJsonSerializator<T> serializator)
        {
            return new ListMapper<List<T>, List<T>, T>(serializator);
        }
    }

    internal sealed class CollectionMapperBuilder : ListMapperBuilderBase
    {
        protected override Type GenericTypeDefinition => typeof(Collection<>);

        protected override Type[] GetMethodTypes(Type type, Type[] types)
        {
            return types;
        }

        private IJsonMapper<Collection<T>> BuildReflectionInvoke<T>(IJsonSerializator<T> serializator)
        {
            return new ListMapper<Collection<T>, Collection<T>, T>(serializator);
        }
    }

    internal sealed class ObservableCollectionMapperBuilder : ListMapperBuilderBase
    {
        protected override Type GenericTypeDefinition => typeof(ObservableCollection<>);

        protected override Type[] GetMethodTypes(Type type, Type[] types)
        {
            return types;
        }

        private IJsonMapper<ObservableCollection<T>> BuildReflectionInvoke<T>(IJsonSerializator<T> serializator)
        {
            return new ListMapper<ObservableCollection<T>, ObservableCollection<T>, T>(serializator);
        }
    }
}