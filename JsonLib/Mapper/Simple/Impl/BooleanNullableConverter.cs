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
    internal sealed class BooleanNullableConverter : ConverterBase<bool?>
    {
        public override void ToJson(IJsonSerializators context, IJsonWriter writer, bool? instance)
        {
            writer.WriteRaw(instance.HasValue ? (instance.Value ? True : False) : Null);
        }

        public override void FromJson(IJsonSerializators context, IJsonReader reader, out bool? instance)
        {
            switch (reader.GetNextToken())
            {
                case JsonToken.False:
                    instance = false;
                    break;
                case JsonToken.True:
                    instance = true;
                    break;
                case JsonToken.Null:
                    instance = null;
                    break;
                default:
                    instance = ThrowInvalidJsonException<bool?>();
                    break;
            }
        }
    }
}