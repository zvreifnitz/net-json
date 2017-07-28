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

        public override string GetStringValue()
        {
            return _value;
        }

        public override int GetIntValue()
        {
            return int.TryParse(
                _value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value)
                ? value
                : ExceptionHelper.ThrowNumberParsingFailException<int>();
        }

        public override long GetLongValue()
        {
            return long.TryParse(
                _value, NumberStyles.Integer, CultureInfo.InvariantCulture, out long value)
                ? value
                : ExceptionHelper.ThrowNumberParsingFailException<long>();
        }

        public override double GetDoubleValue()
        {
            return double.TryParse(
                _value, NumberStyles.Float, CultureInfo.InvariantCulture, out double value)
                ? value
                : ExceptionHelper.ThrowNumberParsingFailException<double>();
        }

        public override decimal GetDecimalValue()
        {
            return decimal.TryParse(
                _value, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal value)
                ? value
                : ExceptionHelper.ThrowNumberParsingFailException<decimal>();
        }

        public override BigInteger GetBigIntegerValue()
        {
            return BigInteger.TryParse(
                _value, NumberStyles.Integer, CultureInfo.InvariantCulture, out BigInteger value)
                ? value
                : ExceptionHelper.ThrowNumberParsingFailException<BigInteger>();
        }

        internal override void ToJson(
            IJsonContext context, IJsonWriter writer, IJsonMapper<JsonElement> elMapper)
        {
            writer.WriteRaw(_value);
        }
    }
}