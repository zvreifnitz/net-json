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
    using System.Collections.Generic;
    using System.Numerics;
    using Helper;

    public abstract class JsonElement
    {
        public abstract JsonElementType Type { get; }

        public virtual string GetStringValue()
        {
            return ExceptionHelper.ThrowJsonTypeMismatchException<string>(Type, JsonElementType.String);
        }

        public virtual List<JsonElement> GetArrayElements()
        {
            return ExceptionHelper.ThrowJsonTypeMismatchException<List<JsonElement>>(Type, JsonElementType.Array);
        }

        public virtual Dictionary<string, JsonElement> GetObjectMembers()
        {
            return ExceptionHelper.ThrowJsonTypeMismatchException<Dictionary<string, JsonElement>>(
                Type, JsonElementType.Object);
        }

        public virtual bool GetBooleanValue()
        {
            return ExceptionHelper.ThrowJsonTypeMismatchException<bool>(Type, JsonElementType.Boolean);
        }

        public virtual int GetIntValue()
        {
            return ExceptionHelper.ThrowJsonTypeMismatchException<int>(Type, JsonElementType.Number);
        }

        public virtual long GetLongValue()
        {
            return ExceptionHelper.ThrowJsonTypeMismatchException<long>(Type, JsonElementType.Number);
        }

        public virtual double GetDoubleValue()
        {
            return ExceptionHelper.ThrowJsonTypeMismatchException<double>(Type, JsonElementType.Number);
        }

        public virtual decimal GetDecimalValue()
        {
            return ExceptionHelper.ThrowJsonTypeMismatchException<decimal>(Type, JsonElementType.Number);
        }

        public virtual BigInteger GetBigIntegerValue()
        {
            return ExceptionHelper.ThrowJsonTypeMismatchException<BigInteger>(Type, JsonElementType.Number);
        }

        internal abstract void ToJson(
            IJsonContext context, IJsonWriter writer, IJsonMapper<JsonElement> elMapper);
    }

    public enum JsonElementType
    {
        Array,
        Boolean,
        Null,
        Number,
        Object,
        String
    }
}