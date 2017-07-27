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
        
        internal static void RegisterDefaultMappers(this JsonSerializationContext context)
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

        private static readonly IJsonMapperBuilder ArrayMapperBuilder = new ArrayMapperBuilder();
        private static readonly IJsonMapperBuilder NullableMapperBuilder = new NullableMapperBuilder();
        private static readonly IJsonMapperBuilder ItfListMapperBuilder = new ItfListMapperBuilder();
        private static readonly IJsonMapperBuilder ListMapperBuilder = new ListMapperBuilder();
        private static readonly IJsonMapperBuilder ItfSetMapperBuilder = new ItfSetMapperBuilder();
        private static readonly IJsonMapperBuilder HashSetMapperBuilder = new HashSetMapperBuilder();
        private static readonly IJsonMapperBuilder SortedSetMapperBuilder = new SortedSetMapperBuilder();
        private static readonly IJsonMapperBuilder ItfDictionaryMapperBuilder = new ItfDictionaryMapperBuilder();
        private static readonly IJsonMapperBuilder DictionaryMapperBuilder = new DictionaryMapperBuilder();
        private static readonly IJsonMapperBuilder ConcurrentDictionaryMapperBuilder = new ConcurrentDictionaryMapperBuilder();
        private static readonly IJsonMapperBuilder SortedDictionaryMapperBuilder = new SortedDictionaryMapperBuilder();
        private static readonly IJsonMapperBuilder SortedListMapperBuilder = new SortedListMapperBuilder();
        private static readonly IJsonMapperBuilder CollectionMapperBuilder = new CollectionMapperBuilder();
        private static readonly IJsonMapperBuilder ObservableCollectionMapperBuilder = new ObservableCollectionMapperBuilder();
        
        internal static void RegisterDefaultBuilders(this JsonSerializationContext context)
        {
            context.RegisterMapperBulder(ArrayMapperBuilder);
            context.RegisterMapperBulder(NullableMapperBuilder);
            context.RegisterMapperBulder(ItfListMapperBuilder);
            context.RegisterMapperBulder(ListMapperBuilder);
            context.RegisterMapperBulder(ItfSetMapperBuilder);
            context.RegisterMapperBulder(HashSetMapperBuilder);
            context.RegisterMapperBulder(SortedSetMapperBuilder);
            context.RegisterMapperBulder(ItfDictionaryMapperBuilder);
            context.RegisterMapperBulder(DictionaryMapperBuilder);
            context.RegisterMapperBulder(ConcurrentDictionaryMapperBuilder);
            context.RegisterMapperBulder(SortedDictionaryMapperBuilder);
            context.RegisterMapperBulder(SortedListMapperBuilder);
            context.RegisterMapperBulder(CollectionMapperBuilder);
            context.RegisterMapperBulder(ObservableCollectionMapperBuilder);
        }
    }
}