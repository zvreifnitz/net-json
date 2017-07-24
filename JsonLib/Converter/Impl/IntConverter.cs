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
    internal sealed class IntConverter : ConverterBase<int>
    {
        public override void ToJson(IJsonSerializators context, IJsonWriter writer, int instance)
        {
            writer.WriteRaw(instance.ToString(DefaultCultureInfo));
        }

        public override void FromJson(IJsonSerializators context, IJsonReader reader, ref int instance)
        {
            if (reader.GetNextToken() != JsonToken.Number ||
                !int.TryParse(reader.ReadValue(), IntegerNumberStyle, DefaultCultureInfo, out instance))
            {
                ThrowInvalidJsonException<object>();
            }
        }

        public override int NewInstance()
        {
            return new int();
        }
    }
}