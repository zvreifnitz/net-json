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

    internal static class JsonMappers
    {
        private static readonly IJsonMapper<BigInteger> BigIntegerMapper = new JsonSimpleMapper<BigInteger>(new BigIntegerConverter());
        private static readonly IJsonMapper<BigInteger?> BigIntegerNullableMapper = new JsonSimpleMapper<BigInteger?>(new BigIntegerNullableConverter());
        private static readonly IJsonMapper<bool> BooleanMapper = new JsonSimpleMapper<bool>(new BooleanConverter());
        private static readonly IJsonMapper<bool?> BooleanNullableMapper = new JsonSimpleMapper<bool?>(new BooleanNullableConverter());
        private static readonly IJsonMapper<byte> ByteMapper = new JsonSimpleMapper<byte>(new ByteConverter());
        private static readonly IJsonMapper<byte?> ByteNullableMapper = new JsonSimpleMapper<byte?>(new ByteNullableConverter());
        private static readonly IJsonMapper<char> CharMapper = new JsonSimpleMapper<char>(new CharConverter());
        private static readonly IJsonMapper<char?> CharNullableMapper = new JsonSimpleMapper<char?>(new CharNullableConverter());
        private static readonly IJsonMapper<DateTime> DateTimeMapper = new JsonSimpleMapper<DateTime>(new DateTimeConverter());
        private static readonly IJsonMapper<DateTime?> DateTimeNullableMapper = new JsonSimpleMapper<DateTime?>(new DateTimeNullableConverter());
        private static readonly IJsonMapper<DateTimeOffset> DateTimeOffsetMapper = new JsonSimpleMapper<DateTimeOffset>(new DateTimeOffsetConverter());
        private static readonly IJsonMapper<DateTimeOffset?> DateTimeOffsetNullableMapper = new JsonSimpleMapper<DateTimeOffset?>(new DateTimeOffsetNullableConverter());
        private static readonly IJsonMapper<decimal> DecimalMapper = new JsonSimpleMapper<decimal>(new DecimalConverter());
        private static readonly IJsonMapper<decimal?> DecimalNullableMapper = new JsonSimpleMapper<decimal?>(new DecimalNullableConverter());
        private static readonly IJsonMapper<double> DoubleMapper = new JsonSimpleMapper<double>(new DoubleConverter());
        private static readonly IJsonMapper<double?> DoubleNullableMapper = new JsonSimpleMapper<double?>(new DoubleNullableConverter());
        private static readonly IJsonMapper<Guid> GuidMapper = new JsonSimpleMapper<Guid>(new GuidConverter());
        private static readonly IJsonMapper<Guid?> GuidNullableMapper = new JsonSimpleMapper<Guid?>(new GuidNullableConverter());
        private static readonly IJsonMapper<int> IntMapper = new JsonSimpleMapper<int>(new IntConverter());
        private static readonly IJsonMapper<int?> IntNullableMapper = new JsonSimpleMapper<int?>(new IntNullableConverter());
        private static readonly IJsonMapper<long> LongMapper = new JsonSimpleMapper<long>(new LongConverter());
        private static readonly IJsonMapper<long?> LongNullableMapper = new JsonSimpleMapper<long?>(new LongNullableConverter());
        private static readonly IJsonMapper<sbyte> SByteMapper = new JsonSimpleMapper<sbyte>(new SByteConverter());
        private static readonly IJsonMapper<sbyte?> SByteNullableMapper = new JsonSimpleMapper<sbyte?>(new SByteNullableConverter());
        private static readonly IJsonMapper<short> ShortMapper = new JsonSimpleMapper<short>(new ShortConverter());
        private static readonly IJsonMapper<short?> ShortNullableMapper = new JsonSimpleMapper<short?>(new ShortNullableConverter());
        private static readonly IJsonMapper<float> SingleMapper = new JsonSimpleMapper<float>(new SingleConverter());
        private static readonly IJsonMapper<float?> SingleNullableMapper = new JsonSimpleMapper<float?>(new SingleNullableConverter());
        private static readonly IJsonMapper<string> StringMapper = new JsonSimpleMapper<string>(new StringConverter());
        private static readonly IJsonMapper<TimeSpan> TimeSpanMapper = new JsonSimpleMapper<TimeSpan>(new TimeSpanConverter());
        private static readonly IJsonMapper<TimeSpan?> TimeSpanNullableMapper = new JsonSimpleMapper<TimeSpan?>(new TimeSpanNullableConverter());
        private static readonly IJsonMapper<uint> UIntMapper = new JsonSimpleMapper<uint>(new UIntConverter());
        private static readonly IJsonMapper<uint?> UIntNullableMapper = new JsonSimpleMapper<uint?>(new UIntNullableConverter());
        private static readonly IJsonMapper<ulong> ULongMapper = new JsonSimpleMapper<ulong>(new ULongConverter());
        private static readonly IJsonMapper<ulong?> ULongNullableMapper = new JsonSimpleMapper<ulong?>(new ULongNullableConverter());
        private static readonly IJsonMapper<ushort> UShortMapper = new JsonSimpleMapper<ushort>(new UShortConverter());
        private static readonly IJsonMapper<ushort?> UShortNullableMapper = new JsonSimpleMapper<ushort?>(new UShortNullableConverter());
        
        internal static void RegisterDefaultMappers(this JsonSerializationContext context)
        {
            context.RegisterMapper(BigIntegerMapper);
            context.RegisterMapper(BigIntegerNullableMapper);
            context.RegisterMapper(BooleanMapper);
            context.RegisterMapper(BooleanNullableMapper);
            context.RegisterMapper(ByteMapper);
            context.RegisterMapper(ByteNullableMapper);
            context.RegisterMapper(CharMapper);
            context.RegisterMapper(CharNullableMapper);
            context.RegisterMapper(DateTimeMapper);
            context.RegisterMapper(DateTimeNullableMapper);
            context.RegisterMapper(DateTimeOffsetMapper);
            context.RegisterMapper(DateTimeOffsetNullableMapper);
            context.RegisterMapper(DecimalMapper);
            context.RegisterMapper(DecimalNullableMapper);
            context.RegisterMapper(DoubleMapper);
            context.RegisterMapper(DoubleNullableMapper);
            context.RegisterMapper(GuidMapper);
            context.RegisterMapper(GuidNullableMapper);
            context.RegisterMapper(IntMapper);
            context.RegisterMapper(IntNullableMapper);
            context.RegisterMapper(LongMapper);
            context.RegisterMapper(LongNullableMapper);
            context.RegisterMapper(SByteMapper);
            context.RegisterMapper(SByteNullableMapper);
            context.RegisterMapper(ShortMapper);
            context.RegisterMapper(ShortNullableMapper);
            context.RegisterMapper(SingleMapper);
            context.RegisterMapper(SingleNullableMapper);
            context.RegisterMapper(StringMapper);
            context.RegisterMapper(TimeSpanMapper);
            context.RegisterMapper(TimeSpanNullableMapper);
            context.RegisterMapper(UIntMapper);
            context.RegisterMapper(UIntNullableMapper);
            context.RegisterMapper(ULongMapper);
            context.RegisterMapper(ULongNullableMapper);
            context.RegisterMapper(UShortMapper);
            context.RegisterMapper(UShortNullableMapper);
        }

        private static readonly IJsonMapperBuilder ItfListMapperBuilder = new ItfListMapperBuilder();
        private static readonly IJsonMapperBuilder ListMapperBuilder = new ListMapperBuilder();
        private static readonly IJsonMapperBuilder LinkedListMapperBuilder = new LinkedListMapperBuilder();
        private static readonly IJsonMapperBuilder QueueMapperBuilder = new QueueMapperBuilder();
        private static readonly IJsonMapperBuilder StackMapperBuilder = new StackMapperBuilder();
        
        internal static void RegisterDefaultBuilders(this JsonSerializationContext context)
        {
            context.RegisterMapperBulder(ItfListMapperBuilder);
            context.RegisterMapperBulder(ListMapperBuilder);
            context.RegisterMapperBulder(LinkedListMapperBuilder);
            context.RegisterMapperBulder(QueueMapperBuilder); 
            context.RegisterMapperBulder(StackMapperBuilder);
        }
    }
}