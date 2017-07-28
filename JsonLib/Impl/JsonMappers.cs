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

namespace com.github.zvreifnitz.JsonLib.Impl
{
    using System;
    using System.Numerics;
    using Mapper.Simple;
    using Mapper.Simple.Impl;
    using Mapper.Collection;
    using Mapper.Common;
    using Json;
    using Mapper.Json;
    
    internal static class JsonMappers
    {
        private static readonly IJsonMapper<JsonElement> JsonElementMapper = new JsonElementMapper();
        private static readonly IJsonMapper<JsonArray> JsonArrayMapper = new JsonArrayMapper();
        private static readonly IJsonMapper<JsonBoolean> JsonBooleanMapper = new JsonBooleanMapper();
        private static readonly IJsonMapper<JsonNull> JsonNullMapper = new JsonNullMapper();
        private static readonly IJsonMapper<JsonNumber> JsonNumberMapper = new JsonNumberMapper();
        private static readonly IJsonMapper<JsonObject> JsonObjectMapper = new JsonObjectMapper();
        private static readonly IJsonMapper<JsonString> JsonStringMapper = new JsonStringMapper();
        private static readonly IJsonMapper<BigInteger> BigIntegerMapper = new JsonSimpleMapper<BigInteger>(new BigIntegerConverter());
        private static readonly IJsonMapper<bool> BooleanMapper = new JsonSimpleMapper<bool>(new BooleanConverter());
        private static readonly IJsonMapper<byte> ByteMapper = new JsonSimpleMapper<byte>(new ByteConverter());
        private static readonly IJsonMapper<char> CharMapper = new JsonSimpleMapper<char>(new CharConverter());
        private static readonly IJsonMapper<DateTime> DateTimeMapper = new JsonSimpleMapper<DateTime>(new DateTimeConverter());
        private static readonly IJsonMapper<DateTimeOffset> DateTimeOffsetMapper = new JsonSimpleMapper<DateTimeOffset>(new DateTimeOffsetConverter());
        private static readonly IJsonMapper<decimal> DecimalMapper = new JsonSimpleMapper<decimal>(new DecimalConverter());
        private static readonly IJsonMapper<double> DoubleMapper = new JsonSimpleMapper<double>(new DoubleConverter());
        private static readonly IJsonMapper<Guid> GuidMapper = new JsonSimpleMapper<Guid>(new GuidConverter());
        private static readonly IJsonMapper<int> IntMapper = new JsonSimpleMapper<int>(new IntConverter());
        private static readonly IJsonMapper<long> LongMapper = new JsonSimpleMapper<long>(new LongConverter());
        private static readonly IJsonMapper<sbyte> SByteMapper = new JsonSimpleMapper<sbyte>(new SByteConverter());
        private static readonly IJsonMapper<short> ShortMapper = new JsonSimpleMapper<short>(new ShortConverter());
        private static readonly IJsonMapper<float> SingleMapper = new JsonSimpleMapper<float>(new SingleConverter());
        private static readonly IJsonMapper<string> StringMapper = new JsonSimpleMapper<string>(new StringConverter());
        private static readonly IJsonMapper<TimeSpan> TimeSpanMapper = new JsonSimpleMapper<TimeSpan>(new TimeSpanConverter());
        private static readonly IJsonMapper<uint> UIntMapper = new JsonSimpleMapper<uint>(new UIntConverter());
        private static readonly IJsonMapper<ulong> ULongMapper = new JsonSimpleMapper<ulong>(new ULongConverter());
        private static readonly IJsonMapper<ushort> UShortMapper = new JsonSimpleMapper<ushort>(new UShortConverter());
        
        internal static void RegisterDefaultMappers(this JsonContextBuilder context)
        {
            context.RegisterMapper(JsonElementMapper);
            context.RegisterMapper(JsonArrayMapper);
            context.RegisterMapper(JsonBooleanMapper);
            context.RegisterMapper(JsonNullMapper);
            context.RegisterMapper(JsonNumberMapper);
            context.RegisterMapper(JsonObjectMapper);
            context.RegisterMapper(JsonStringMapper);
            context.RegisterMapper(BigIntegerMapper);
            context.RegisterMapper(BooleanMapper);
            context.RegisterMapper(ByteMapper);
            context.RegisterMapper(CharMapper);
            context.RegisterMapper(DateTimeMapper);
            context.RegisterMapper(DateTimeOffsetMapper);
            context.RegisterMapper(DecimalMapper);
            context.RegisterMapper(DoubleMapper);
            context.RegisterMapper(GuidMapper);
            context.RegisterMapper(IntMapper);
            context.RegisterMapper(LongMapper);
            context.RegisterMapper(SByteMapper);
            context.RegisterMapper(ShortMapper);
            context.RegisterMapper(SingleMapper);
            context.RegisterMapper(StringMapper);
            context.RegisterMapper(TimeSpanMapper);
            context.RegisterMapper(UIntMapper);
            context.RegisterMapper(ULongMapper);
            context.RegisterMapper(UShortMapper);
        }

        private static readonly IRuntimeMapperBuilder ArrayMapperBuilder = new ArrayMapperBuilder();
        private static readonly IRuntimeMapperBuilder NullableMapperBuilder = new NullableMapperBuilder();
        private static readonly IRuntimeMapperBuilder ItfListMapperBuilder = new ItfListMapperBuilder();
        private static readonly IRuntimeMapperBuilder ListMapperBuilder = new ListMapperBuilder();
        private static readonly IRuntimeMapperBuilder ItfSetMapperBuilder = new ItfSetMapperBuilder();
        private static readonly IRuntimeMapperBuilder HashSetMapperBuilder = new HashSetMapperBuilder();
        private static readonly IRuntimeMapperBuilder SortedSetMapperBuilder = new SortedSetMapperBuilder();
        private static readonly IRuntimeMapperBuilder ItfDictionaryMapperBuilder = new ItfDictionaryMapperBuilder();
        private static readonly IRuntimeMapperBuilder DictionaryMapperBuilder = new DictionaryMapperBuilder();
        private static readonly IRuntimeMapperBuilder ConcurrentDictionaryMapperBuilder = new ConcurrentDictionaryMapperBuilder();
        private static readonly IRuntimeMapperBuilder SortedDictionaryMapperBuilder = new SortedDictionaryMapperBuilder();
        private static readonly IRuntimeMapperBuilder SortedListMapperBuilder = new SortedListMapperBuilder();
        private static readonly IRuntimeMapperBuilder CollectionMapperBuilder = new CollectionMapperBuilder();
        private static readonly IRuntimeMapperBuilder ObservableCollectionMapperBuilder = new ObservableCollectionMapperBuilder();
        
        internal static void RegisterDefaultBuilders(this JsonContextBuilder context)
        {
            context.RegisterBuilder(ArrayMapperBuilder);
            context.RegisterBuilder(NullableMapperBuilder);
            context.RegisterBuilder(ItfListMapperBuilder);
            context.RegisterBuilder(ListMapperBuilder);
            context.RegisterBuilder(ItfSetMapperBuilder);
            context.RegisterBuilder(HashSetMapperBuilder);
            context.RegisterBuilder(SortedSetMapperBuilder);
            context.RegisterBuilder(ItfDictionaryMapperBuilder);
            context.RegisterBuilder(DictionaryMapperBuilder);
            context.RegisterBuilder(ConcurrentDictionaryMapperBuilder);
            context.RegisterBuilder(SortedDictionaryMapperBuilder);
            context.RegisterBuilder(SortedListMapperBuilder);
            context.RegisterBuilder(CollectionMapperBuilder);
            context.RegisterBuilder(ObservableCollectionMapperBuilder);
        }
    }
}