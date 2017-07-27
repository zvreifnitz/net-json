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
        public abstract JsonType Type { get; }

        public virtual string GetStringValue()
        {
            return ExceptionHelper.ThrowJsonTypeMismatchException<string>(Type, JsonType.String);
        }

        public virtual List<JsonElement> GetArrayElements()
        {
            return ExceptionHelper.ThrowJsonTypeMismatchException<List<JsonElement>>(Type, JsonType.Array);
        }

        public virtual Dictionary<string, JsonElement> GetObjectMembers()
        {
            return ExceptionHelper.ThrowJsonTypeMismatchException<Dictionary<string, JsonElement>>(
                Type, JsonType.Object);
        }

        public virtual bool GetBooleanValue()
        {
            return ExceptionHelper.ThrowJsonTypeMismatchException<bool>(Type, JsonType.Boolean);
        }

        public virtual int GetIntValue()
        {
            return ExceptionHelper.ThrowJsonTypeMismatchException<int>(Type, JsonType.Number);
        }

        public virtual long GetLongValue()
        {
            return ExceptionHelper.ThrowJsonTypeMismatchException<long>(Type, JsonType.Number);
        }

        public virtual double GetDoubleValue()
        {
            return ExceptionHelper.ThrowJsonTypeMismatchException<double>(Type, JsonType.Number);
        }

        public virtual decimal GetDecimalValue()
        {
            return ExceptionHelper.ThrowJsonTypeMismatchException<decimal>(Type, JsonType.Number);
        }

        public virtual BigInteger GetBigIntegerValue()
        {
            return ExceptionHelper.ThrowJsonTypeMismatchException<BigInteger>(Type, JsonType.Number);
        }

        internal abstract void ToJson(
            IJsonSerializators context, IJsonWriter writer, IJsonMapper<JsonElement> elMapper);
    }

    public enum JsonType
    {
        Array,
        Boolean,
        Null,
        Number,
        Object,
        String
    }
}