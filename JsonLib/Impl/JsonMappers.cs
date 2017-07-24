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
    using Converter;
    using Converter.Impl;
    using System;
    using System.Numerics;

    internal static class JsonMappers
    {
        private static readonly IJsonMapper<BigInteger> BigIntegerMapper = new ConverterJsonMapper<BigInteger>(new BigIntegerConverter());
        private static readonly IJsonMapper<BigInteger?> BigIntegerNullableMapper = new ConverterJsonMapper<BigInteger?>(new BigIntegerNullableConverter());
        private static readonly IJsonMapper<bool> BooleanMapper = new ConverterJsonMapper<bool>(new BooleanConverter());
        private static readonly IJsonMapper<bool?> BooleanNullableMapper = new ConverterJsonMapper<bool?>(new BooleanNullableConverter());
        private static readonly IJsonMapper<byte> ByteMapper = new ConverterJsonMapper<byte>(new ByteConverter());
        private static readonly IJsonMapper<byte?> ByteNullableMapper = new ConverterJsonMapper<byte?>(new ByteNullableConverter());
        private static readonly IJsonMapper<char> CharMapper = new ConverterJsonMapper<char>(new CharConverter());
        private static readonly IJsonMapper<char?> CharNullableMapper = new ConverterJsonMapper<char?>(new CharNullableConverter());
        private static readonly IJsonMapper<DateTime> DateTimeMapper = new ConverterJsonMapper<DateTime>(new DateTimeConverter());
        private static readonly IJsonMapper<DateTime?> DateTimeNullableMapper = new ConverterJsonMapper<DateTime?>(new DateTimeNullableConverter());
        private static readonly IJsonMapper<DateTimeOffset> DateTimeOffsetMapper = new ConverterJsonMapper<DateTimeOffset>(new DateTimeOffsetConverter());
        private static readonly IJsonMapper<DateTimeOffset?> DateTimeOffsetNullableMapper = new ConverterJsonMapper<DateTimeOffset?>(new DateTimeOffsetNullableConverter());
        private static readonly IJsonMapper<decimal> DecimalMapper = new ConverterJsonMapper<decimal>(new DecimalConverter());
        private static readonly IJsonMapper<decimal?> DecimalNullableMapper = new ConverterJsonMapper<decimal?>(new DecimalNullableConverter());
        private static readonly IJsonMapper<double> DoubleMapper = new ConverterJsonMapper<double>(new DoubleConverter());
        private static readonly IJsonMapper<double?> DoubleNullableMapper = new ConverterJsonMapper<double?>(new DoubleNullableConverter());
        private static readonly IJsonMapper<Guid> GuidMapper = new ConverterJsonMapper<Guid>(new GuidConverter());
        private static readonly IJsonMapper<Guid?> GuidNullableMapper = new ConverterJsonMapper<Guid?>(new GuidNullableConverter());
        private static readonly IJsonMapper<int> IntMapper = new ConverterJsonMapper<int>(new IntConverter());
        private static readonly IJsonMapper<int?> IntNullableMapper = new ConverterJsonMapper<int?>(new IntNullableConverter());
        private static readonly IJsonMapper<long> LongMapper = new ConverterJsonMapper<long>(new LongConverter());
        private static readonly IJsonMapper<long?> LongNullableMapper = new ConverterJsonMapper<long?>(new LongNullableConverter());
        private static readonly IJsonMapper<sbyte> SByteMapper = new ConverterJsonMapper<sbyte>(new SByteConverter());
        private static readonly IJsonMapper<sbyte?> SByteNullableMapper = new ConverterJsonMapper<sbyte?>(new SByteNullableConverter());
        private static readonly IJsonMapper<short> ShortMapper = new ConverterJsonMapper<short>(new ShortConverter());
        private static readonly IJsonMapper<short?> ShortNullableMapper = new ConverterJsonMapper<short?>(new ShortNullableConverter());
        private static readonly IJsonMapper<float> SingleMapper = new ConverterJsonMapper<float>(new SingleConverter());
        private static readonly IJsonMapper<float?> SingleNullableMapper = new ConverterJsonMapper<float?>(new SingleNullableConverter());
        private static readonly IJsonMapper<string> StringMapper = new ConverterJsonMapper<string>(new StringConverter());
        private static readonly IJsonMapper<TimeSpan> TimeSpanMapper = new ConverterJsonMapper<TimeSpan>(new TimeSpanConverter());
        private static readonly IJsonMapper<TimeSpan?> TimeSpanNullableMapper = new ConverterJsonMapper<TimeSpan?>(new TimeSpanNullableConverter());
        private static readonly IJsonMapper<uint> UIntMapper = new ConverterJsonMapper<uint>(new UIntConverter());
        private static readonly IJsonMapper<uint?> UIntNullableMapper = new ConverterJsonMapper<uint?>(new UIntNullableConverter());
        private static readonly IJsonMapper<ulong> ULongMapper = new ConverterJsonMapper<ulong>(new ULongConverter());
        private static readonly IJsonMapper<ulong?> ULongNullableMapper = new ConverterJsonMapper<ulong?>(new ULongNullableConverter());
        private static readonly IJsonMapper<ushort> UShortMapper = new ConverterJsonMapper<ushort>(new UShortConverter());
        private static readonly IJsonMapper<ushort?> UShortNullableMapper = new ConverterJsonMapper<ushort?>(new UShortNullableConverter());
        
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
    }
}