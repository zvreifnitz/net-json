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

using System.Globalization;
using com.github.zvreifnitz.JsonLib.Helper;
using com.github.zvreifnitz.JsonLib.Parser;

namespace com.github.zvreifnitz.JsonLib.Mapper.Simple.Impl
{
    internal abstract class ConverterBase
    {
        internal const string Null = JsonLiterals.Null;
        internal const string True = JsonLiterals.True;
        internal const string False = JsonLiterals.False;

        internal static readonly CultureInfo DefaultCultureInfo = CultureInfo.InvariantCulture;
        internal static readonly NumberStyles IntegerNumberStyle = NumberStyles.Integer;
        internal static readonly NumberStyles FloatNumberStyle = NumberStyles.Float;
    }

    internal abstract class ConverterBase<T> : ConverterBase, ISimpleConverter<T>
    {
        public abstract void ToJson(IJsonSerializators context, IJsonWriter writer, T instance);

        public abstract void FromJson(IJsonSerializators context, IJsonReader reader, out T instance);

        protected TR ThrowInvalidJsonException<TR>()
        {
            return ExceptionHelper.ThrowInvalidJsonException<TR>();
        }
    }
}