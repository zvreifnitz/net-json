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

namespace com.github.zvreifnitz.JsonLib.Mapper.Simple.Impl
{
    internal sealed class UIntConverter : ConverterBase<uint>
    {
        public override void ToJson(IJsonContext context, IJsonWriter writer, uint instance)
        {
            writer.WriteRaw(instance.ToString(DefaultCultureInfo));
        }

        public override void FromJson(IJsonContext context, IJsonReader reader, out uint instance)
        {
            if (reader.GetNextToken() != JsonToken.Number ||
                !uint.TryParse(reader.ReadValue(), IntegerNumberStyle, DefaultCultureInfo, out instance))
            {
                instance = ThrowInvalidJsonException<uint>();
            }
        }
    }
}