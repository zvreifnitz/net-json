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
    using Converter.Impl;
    using System;
    using System.Numerics;

    internal static class JsonMappers
    {
        private static readonly IJsonMapper<BigInteger> BigIntegerMapper = new JsonMapper<BigInteger>(new BigIntegerConverter());
        private static readonly IJsonMapper<BigInteger?> BigIntegerNullableMapper = new JsonMapper<BigInteger?>(new BigIntegerNullableConverter());
        private static readonly IJsonMapper<bool> BooleanMapper = new JsonMapper<bool>(new BooleanConverter());
        private static readonly IJsonMapper<bool?> BooleanNullableMapper = new JsonMapper<bool?>(new BooleanNullableConverter());
        private static readonly IJsonMapper<byte> ByteMapper = new JsonMapper<byte>(new ByteConverter());
        private static readonly IJsonMapper<byte?> ByteNullableMapper = new JsonMapper<byte?>(new ByteNullableConverter());
        private static readonly IJsonMapper<DateTime> DateTimeMapper = new JsonMapper<DateTime>(new DateTimeConverter());
        private static readonly IJsonMapper<DateTime?> DateTimeNullableMapper = new JsonMapper<DateTime?>(new DateTimeNullableConverter());
        private static readonly IJsonMapper<DateTimeOffset> DateTimeOffsetMapper = new JsonMapper<DateTimeOffset>(new DateTimeOffsetConverter());
        private static readonly IJsonMapper<DateTimeOffset?> DateTimeOffsetNullableMapper = new JsonMapper<DateTimeOffset?>(new DateTimeOffsetNullableConverter());
        private static readonly IJsonMapper<decimal> DecimalMapper = new JsonMapper<decimal>(new DecimalConverter());
        private static readonly IJsonMapper<decimal?> DecimalNullableMapper = new JsonMapper<decimal?>(new DecimalNullableConverter());
        private static readonly IJsonMapper<double> DoubleMapper = new JsonMapper<double>(new DoubleConverter());
        private static readonly IJsonMapper<double?> DoubleNullableMapper = new JsonMapper<double?>(new DoubleNullableConverter());
        private static readonly IJsonMapper<Guid> GuidMapper = new JsonMapper<Guid>(new GuidConverter());
        private static readonly IJsonMapper<Guid?> GuidNullableMapper = new JsonMapper<Guid?>(new GuidNullableConverter());
        private static readonly IJsonMapper<int> IntMapper = new JsonMapper<int>(new IntConverter());
        private static readonly IJsonMapper<int?> IntNullableMapper = new JsonMapper<int?>(new IntNullableConverter());
        private static readonly IJsonMapper<long> LongMapper = new JsonMapper<long>(new LongConverter());
        private static readonly IJsonMapper<long?> LongNullableMapper = new JsonMapper<long?>(new LongNullableConverter());
        private static readonly IJsonMapper<sbyte> SByteMapper = new JsonMapper<sbyte>(new SByteConverter());
        private static readonly IJsonMapper<sbyte?> SByteNullableMapper = new JsonMapper<sbyte?>(new SByteNullableConverter());
        private static readonly IJsonMapper<short> ShortMapper = new JsonMapper<short>(new ShortConverter());
        private static readonly IJsonMapper<short?> ShortNullableMapper = new JsonMapper<short?>(new ShortNullableConverter());
        private static readonly IJsonMapper<float> SingleMapper = new JsonMapper<float>(new SingleConverter());
        private static readonly IJsonMapper<float?> SingleNullableMapper = new JsonMapper<float?>(new SingleNullableConverter());
        private static readonly IJsonMapper<string> StringMapper = new JsonMapper<string>(new StringConverter());
        private static readonly IJsonMapper<TimeSpan> TimeSpanMapper = new JsonMapper<TimeSpan>(new TimeSpanConverter());
        private static readonly IJsonMapper<TimeSpan?> TimeSpanNullableMapper = new JsonMapper<TimeSpan?>(new TimeSpanNullableConverter());
        private static readonly IJsonMapper<uint> UIntMapper = new JsonMapper<uint>(new UIntConverter());
        private static readonly IJsonMapper<uint?> UIntNullableMapper = new JsonMapper<uint?>(new UIntNullableConverter());
        private static readonly IJsonMapper<ulong> ULongMapper = new JsonMapper<ulong>(new ULongConverter());
        private static readonly IJsonMapper<ulong?> ULongNullableMapper = new JsonMapper<ulong?>(new ULongNullableConverter());
        private static readonly IJsonMapper<ushort> UShortMapper = new JsonMapper<ushort>(new UShortConverter());
        private static readonly IJsonMapper<ushort?> UShortNullableMapper = new JsonMapper<ushort?>(new UShortNullableConverter());
        
        internal static void RegisterDefaultMappers(this JsonSerializationContext context)
        {
            context.RegisterMapper(BigIntegerMapper);
            context.RegisterMapper(BigIntegerNullableMapper);
            context.RegisterMapper(BooleanMapper);
            context.RegisterMapper(BooleanNullableMapper);
            context.RegisterMapper(ByteMapper);
            context.RegisterMapper(ByteNullableMapper);
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
    }
}