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

namespace com.github.zvreifnitz.JsonLib.Mapper.Json
{
    using System.Globalization;
    using System.Numerics;
    using Helper;
    using JsonLib.Json;

    internal sealed class JsonNumberMapper : JsonElementMapperBase<JsonNumber>
    {
        public override JsonNumber FromJson(IJsonSerializators context, IJsonReader reader)
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