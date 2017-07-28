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

namespace com.github.zvreifnitz.JsonLib.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Json;

    public static class ExceptionHelper
    {
        public static T ThrowJsonTypeMismatchException<T>(JsonElementType jsonTypeThis, JsonElementType jsonTypeOther)
        {
            throw new JsonException(JsonExceptionType.TypeMismatch,
                string.Format("Json value '{0}' could not be converted to '{1}'", jsonTypeThis, jsonTypeOther));
        }

        public static T ThrowInvalidJsonException<T>()
        {
            throw new JsonException(JsonExceptionType.InvalidJson, "Invalid JSON input");
        }
        
        public static T ThrowNumberParsingFailException<T>()
        {
            throw new JsonException(JsonExceptionType.NumberParsingFail, "JSON number parsing failed");
        }

        public static IJsonSerializator<T> ThrowMapperNotRegisteredException<T>()
        {
            throw new JsonException(JsonExceptionType.MapperNotRegistered,
                string.Format("Mapper for type '{0}' is not registered", typeof(T).FullName));
        }

        public static IRuntimeMapperBuilder ThrowNoSuitableBuilderException<T>()
        {
            throw new JsonException(JsonExceptionType.MapperNotRegistered,
                string.Format("Mapper for type '{0}' is not registered", typeof(T).FullName));
        }

        public static IRuntimeMapperBuilder ThrowManyBuildersException<T>(List<IRuntimeMapperBuilder> builders)
        {
            var builderNames = string.Join(", ", builders.Select(b => b.GetType().FullName));
            throw new JsonException(JsonExceptionType.TooManyBuilders, string.Format(
                "Type '{0}' can be built by many builders: [{1}]", typeof(T).FullName,
                builderNames));
        }

        public static T ThrowEndOfStreamException<T>()
        {
            throw new JsonException(JsonExceptionType.EndOfStream, "End of stream");
        }

        public static T ThrowUnexpectedException<T>(Exception exc)
        {
            throw new JsonException(JsonExceptionType.Unexpected, "Unexpected exception", exc);
        }
        
        public static T ThrowInvalidPropertyExpressionException<T>()
        {
            throw new JsonException(JsonExceptionType.InvalidPropertyExpression, "Invalid property expression");
        }
    }
}