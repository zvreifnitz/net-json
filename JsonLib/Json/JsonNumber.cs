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

namespace com.github.zvreifnitz.JsonLib.Json
{
    using System.Globalization;
    using System.Numerics;
    using Helper;

    public sealed class JsonNumber : JsonElement
    {
        private readonly string _value;

        public JsonNumber(int value)
        {
            _value = value.ToString(CultureInfo.InvariantCulture);
        }

        public JsonNumber(long value)
        {
            _value = value.ToString(CultureInfo.InvariantCulture);
        }

        public JsonNumber(double value)
        {
            _value = value.ToString(CultureInfo.InvariantCulture);
        }

        public JsonNumber(decimal value)
        {
            _value = value.ToString(CultureInfo.InvariantCulture);
        }

        public JsonNumber(BigInteger value)
        {
            _value = value.ToString(CultureInfo.InvariantCulture);
        }

        public override JsonType Type => JsonType.Number;

        public override int GetIntValue()
        {
            if (int.TryParse(
                _value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
            {
                return value;
            }
            return ExceptionHelper.ThrowNumberParsingFailException<int>();
        }

        public override long GetLongValue()
        {
            if (long.TryParse(
                _value, NumberStyles.Integer, CultureInfo.InvariantCulture, out long value))
            {
                return value;
            }
            return ExceptionHelper.ThrowNumberParsingFailException<long>();
        }

        public override double GetDoubleValue()
        {
            if (double.TryParse(
                _value, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                return value;
            }
            return ExceptionHelper.ThrowNumberParsingFailException<double>();
        }

        public override decimal GetDecimalValue()
        {
            if (decimal.TryParse(
                _value, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal value))
            {
                return value;
            }
            return ExceptionHelper.ThrowNumberParsingFailException<decimal>();
        }

        public override BigInteger GetBigIntegerValue()
        {
            if (BigInteger.TryParse(
                _value, NumberStyles.Integer, CultureInfo.InvariantCulture, out BigInteger value))
            {
                return value;
            }
            return ExceptionHelper.ThrowNumberParsingFailException<BigInteger>();
        }

        internal override void ToJson(IJsonSerializators context, IJsonWriter writer)
        {
            writer.WriteRaw(_value);
        }

        internal new static JsonNumber FromJson(IJsonSerializators context, IJsonReader reader)
        {
            switch (reader.GetNextToken())
            {
                case JsonToken.Null:
                    return null;
                case JsonToken.Number:
                    var value = reader.ReadValue();
                    if (double.TryParse(
                        value, NumberStyles.Float, CultureInfo.InvariantCulture, out double doubleOut))
                    {
                        return new JsonNumber(doubleOut);
                    }
                    if (long.TryParse(
                        value, NumberStyles.Integer, CultureInfo.InvariantCulture, out long longOut))
                    {
                        return new JsonNumber(longOut);
                    }
                    if (BigInteger.TryParse(
                        value, NumberStyles.Integer, CultureInfo.InvariantCulture, out BigInteger bigIntegerOut))
                    {
                        return new JsonNumber(bigIntegerOut);
                    }
                    return ExceptionHelper.ThrowNumberParsingFailException<JsonNumber>();
                default:
                    return ExceptionHelper.ThrowInvalidJsonException<JsonNumber>();
            }
        }
    }
}