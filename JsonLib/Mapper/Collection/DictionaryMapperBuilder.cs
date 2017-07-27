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
    using System.Collections.Concurrent;
    using Helper;
    using Parser;
    using Common;

    internal abstract class DictionaryMapperBuilderBase : MapperBuilderBase
    {
        protected static readonly Type StringType = typeof(string);

        protected abstract Type GenericTypeDefinition { get; }

        protected sealed override bool IsRequestedTypeSupported(Type type)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == GenericTypeDefinition;
        }

        protected abstract class
            DictionaryMapperBase<TDictionaryImpl, TDictionary, TKey, TValue> : IJsonMapper<TDictionary>
            where TDictionary : IDictionary<TKey, TValue>
            where TDictionaryImpl : TDictionary, new()
        {
            protected abstract void ToJsonItem(
                IJsonSerializators context, IJsonWriter writer, KeyValuePair<TKey, TValue> instance);

            protected abstract KeyValuePair<TKey, TValue> FromJsonItem(IJsonSerializators context, IJsonReader reader);

            private readonly IJsonMapper<TKey> _keyMapper;
            private readonly IJsonMapper<TValue> _valueMapper;
            private readonly string _startLiteral;
            private readonly string _endLiteral;
            private readonly JsonToken _startToken;
            private readonly JsonToken _endToken;

            protected DictionaryMapperBase(
                IJsonSerializator<TKey> keySerializator, IJsonSerializator<TValue> valueSerializator,
                string startLiteral, string endLiteral, JsonToken startToken, JsonToken endToken)
            {
                _keyMapper = keySerializator.Mapper;
                _valueMapper = valueSerializator.Mapper;
                _startLiteral = startLiteral;
                _endLiteral = endLiteral;
                _startToken = startToken;
                _endToken = endToken;
            }

            public bool CanSerialize => true;

            public bool CanDeserialize => true;

            public void Init(IJsonSerializators context)
            {
            }

            public void ToJson(IJsonSerializators context, IJsonWriter writer, TDictionary instance)
            {
                if (instance == null)
                {
                    writer.WriteRaw(JsonLiterals.Null);
                }
                else
                {
                    writer.WriteRaw(_startLiteral);
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
                    writer.WriteRaw(_endLiteral);
                }
            }

            public TDictionary FromJson(IJsonSerializators context, IJsonReader reader)
            {
                JsonToken token = reader.GetNextToken();
                if (token == JsonToken.Null)
                {
                    return default(TDictionary);
                }
                if (token != _startToken)
                {
                    return ExceptionHelper.ThrowInvalidJsonException<TDictionary>();
                }
                TDictionaryImpl result = new TDictionaryImpl();
                while (true)
                {
                    token = reader.GetNextToken();
                    if (token == _endToken)
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

            protected void ToJsonKey(IJsonSerializators context, IJsonWriter writer, TKey instance)
            {
                if (_keyMapper.CanSerialize)
                {
                    _keyMapper.ToJson(context, writer, instance);
                }
                else
                {
                    writer.WriteRaw(JsonLiterals.Null);
                }
            }

            protected void ToJsonValue(IJsonSerializators context, IJsonWriter writer, TValue instance)
            {
                if (_valueMapper.CanSerialize)
                {
                    _valueMapper.ToJson(context, writer, instance);
                }
                else
                {
                    writer.WriteRaw(JsonLiterals.Null);
                }
            }

            protected TKey FromJsonKey(IJsonSerializators context, IJsonReader reader)
            {
                return _keyMapper.CanDeserialize
                    ? _keyMapper.FromJson(context, reader)
                    : default(TKey);
            }

            protected TValue FromJsonValue(IJsonSerializators context, IJsonReader reader)
            {
                return _valueMapper.CanDeserialize
                    ? _valueMapper.FromJson(context, reader)
                    : default(TValue);
            }
        }

        protected sealed class ObjectDictionaryMapper<TDictionaryImpl, TDictionary, TKey, TValue> :
            DictionaryMapperBase<TDictionaryImpl, TDictionary, TKey, TValue>
            where TDictionary : IDictionary<TKey, TValue>
            where TDictionaryImpl : TDictionary, new()
        {
            public ObjectDictionaryMapper(
                IJsonSerializator<TKey> keySerializator, IJsonSerializator<TValue> valueSerializator) :
                base(
                    keySerializator, valueSerializator,
                    JsonLiterals.ObjectStart, JsonLiterals.ObjectEnd,
                    JsonToken.ObjectStart, JsonToken.ObjectEnd)
            {
            }

            protected override void ToJsonItem(IJsonSerializators context, IJsonWriter writer,
                KeyValuePair<TKey, TValue> instance)
            {
                ToJsonKey(context, writer, instance.Key);
                writer.WriteRaw(JsonLiterals.Colon);
                ToJsonValue(context, writer, instance.Value);
            }

            protected override KeyValuePair<TKey, TValue> FromJsonItem(IJsonSerializators context, IJsonReader reader)
            {
                TKey key = FromJsonKey(context, reader);
                JsonSerializatorsHelper.ThrowIfNotMatch(reader, JsonToken.Colon);
                TValue value = FromJsonValue(context, reader);
                return new KeyValuePair<TKey, TValue>(key, value);
            }
        }

        protected sealed class ArrayDictionaryMapper<TDictionaryImpl, TDictionary, TKey, TValue> :
            DictionaryMapperBase<TDictionaryImpl, TDictionary, TKey, TValue>
            where TDictionary : IDictionary<TKey, TValue>
            where TDictionaryImpl : TDictionary, new()
        {
            public ArrayDictionaryMapper(
                IJsonSerializator<TKey> keySerializator, IJsonSerializator<TValue> valueSerializator) :
                base(
                    keySerializator, valueSerializator,
                    JsonLiterals.ArrayStart, JsonLiterals.ArrayEnd,
                    JsonToken.ArrayStart, JsonToken.ArrayEnd)
            {
            }

            protected override void ToJsonItem(IJsonSerializators context, IJsonWriter writer,
                KeyValuePair<TKey, TValue> instance)
            {
                writer.WriteRaw(JsonLiterals.ArrayStart);
                ToJsonKey(context, writer, instance.Key);
                writer.WriteRaw(JsonLiterals.Comma);
                ToJsonValue(context, writer, instance.Value);
                writer.WriteRaw(JsonLiterals.ArrayEnd);
            }

            protected override KeyValuePair<TKey, TValue> FromJsonItem(IJsonSerializators context, IJsonReader reader)
            {
                JsonSerializatorsHelper.ThrowIfNotMatch(reader, JsonToken.ArrayStart);
                TKey key = FromJsonKey(context, reader);
                JsonSerializatorsHelper.ThrowIfNotMatch(reader, JsonToken.Comma);
                TValue value = FromJsonValue(context, reader);
                JsonSerializatorsHelper.ThrowIfNotMatch(reader, JsonToken.ArrayEnd);
                return new KeyValuePair<TKey, TValue>(key, value);
            }
        }
    }

    internal sealed class ItfDictionaryMapperBuilder : DictionaryMapperBuilderBase
    {
        protected override Type GenericTypeDefinition => typeof(IDictionary<,>);

        protected override Type[] GetMethodTypes(Type type, Type[] types)
        {
            return types;
        }

        private IJsonMapper<IDictionary<TKey, TValue>> BuildReflectionInvoke<TKey, TValue>(
            IJsonSerializator<TKey> keySerializator, IJsonSerializator<TValue> valueSerializator)
        {
            if (typeof(TKey) == StringType)
            {
                return new ObjectDictionaryMapper<Dictionary<TKey, TValue>, IDictionary<TKey, TValue>, TKey, TValue>(
                    keySerializator, valueSerializator);
            }
            return new ArrayDictionaryMapper<Dictionary<TKey, TValue>, IDictionary<TKey, TValue>, TKey, TValue>(
                keySerializator, valueSerializator);
        }
    }

    internal sealed class DictionaryMapperBuilder : DictionaryMapperBuilderBase
    {
        protected override Type GenericTypeDefinition => typeof(Dictionary<,>);

        protected override Type[] GetMethodTypes(Type type, Type[] types)
        {
            return types;
        }

        private IJsonMapper<Dictionary<TKey, TValue>> BuildReflectionInvoke<TKey, TValue>(
            IJsonSerializator<TKey> keySerializator, IJsonSerializator<TValue> valueSerializator)
        {
            if (typeof(TKey) == StringType)
            {
                return new ObjectDictionaryMapper<Dictionary<TKey, TValue>, Dictionary<TKey, TValue>, TKey, TValue>(
                    keySerializator, valueSerializator);
            }
            return new ArrayDictionaryMapper<Dictionary<TKey, TValue>, Dictionary<TKey, TValue>, TKey, TValue>(
                keySerializator, valueSerializator);
        }
    }

    internal sealed class ConcurrentDictionaryMapperBuilder : DictionaryMapperBuilderBase
    {
        protected override Type GenericTypeDefinition => typeof(ConcurrentDictionary<,>);

        protected override Type[] GetMethodTypes(Type type, Type[] types)
        {
            return types;
        }

        private IJsonMapper<ConcurrentDictionary<TKey, TValue>> BuildReflectionInvoke<TKey, TValue>(
            IJsonSerializator<TKey> keySerializator, IJsonSerializator<TValue> valueSerializator)
        {
            if (typeof(TKey) == StringType)
            {
                return new ObjectDictionaryMapper<ConcurrentDictionary<TKey, TValue>, ConcurrentDictionary<TKey, TValue>
                    , TKey, TValue>(
                    keySerializator, valueSerializator);
            }
            return new ArrayDictionaryMapper<ConcurrentDictionary<TKey, TValue>, ConcurrentDictionary<TKey, TValue>,
                TKey, TValue>(
                keySerializator, valueSerializator);
        }
    }

    internal sealed class SortedDictionaryMapperBuilder : DictionaryMapperBuilderBase
    {
        protected override Type GenericTypeDefinition => typeof(SortedDictionary<,>);

        protected override Type[] GetMethodTypes(Type type, Type[] types)
        {
            return types;
        }

        private IJsonMapper<SortedDictionary<TKey, TValue>> BuildReflectionInvoke<TKey, TValue>(
            IJsonSerializator<TKey> keySerializator, IJsonSerializator<TValue> valueSerializator)
        {
            if (typeof(TKey) == StringType)
            {
                return new ObjectDictionaryMapper<SortedDictionary<TKey, TValue>, SortedDictionary<TKey, TValue>, TKey,
                    TValue>(
                    keySerializator, valueSerializator);
            }
            return new ArrayDictionaryMapper<SortedDictionary<TKey, TValue>, SortedDictionary<TKey, TValue>, TKey,
                TValue>(
                keySerializator, valueSerializator);
        }
    }

    internal sealed class SortedListMapperBuilder : DictionaryMapperBuilderBase
    {
        protected override Type GenericTypeDefinition => typeof(SortedList<,>);

        protected override Type[] GetMethodTypes(Type type, Type[] types)
        {
            return types;
        }

        private IJsonMapper<SortedList<TKey, TValue>> BuildReflectionInvoke<TKey, TValue>(
            IJsonSerializator<TKey> keySerializator, IJsonSerializator<TValue> valueSerializator)
        {
            if (typeof(TKey) == StringType)
            {
                return new ObjectDictionaryMapper<SortedList<TKey, TValue>, SortedList<TKey, TValue>, TKey, TValue>(
                    keySerializator, valueSerializator);
            }
            return new ArrayDictionaryMapper<SortedList<TKey, TValue>, SortedList<TKey, TValue>, TKey, TValue>(
                keySerializator, valueSerializator);
        }
    }
}