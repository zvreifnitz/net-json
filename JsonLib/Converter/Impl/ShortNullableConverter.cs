﻿/*
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

namespace com.github.zvreifnitz.JsonLib.Converter.Impl
{
    internal sealed class ShortNullableConverter : ConverterBase<short?>
    {
        public override void ToJson(IJsonSerializators context, IJsonWriter writer, short? instance)
        {
            writer.WriteRaw(instance.HasValue ? instance.Value.ToString(DefaultCultureInfo) : Null);
        }

        public override void FromJson(IJsonSerializators context, IJsonReader reader, ref short? instance)
        {
            var token = reader.GetNextToken();
            if (token == JsonToken.Null)
            {
                instance = null;
            }
            else if (token == JsonToken.Number &&
                     short.TryParse(reader.ReadValue(), IntegerNumberStyle, DefaultCultureInfo, out short parsed))
            {
                instance = parsed;
            }
            else
            {
                ThrowInvalidJsonException<object>();
            }
        }

        public override short? NewInstance()
        {
            return null;
        }
    }
}