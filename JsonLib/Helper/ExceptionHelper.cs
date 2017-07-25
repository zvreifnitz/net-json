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

    public static class ExceptionHelper
    {
        public static T ThrowInvalidJsonException<T>()
        {
            throw new JsonException("Invalid JSON input");
        }

        public static IJsonSerializator<T> ThrowMapperNotRegisteredException<T>()
        {
            throw new JsonException(string.Format("Mapper for type '{0}' is not registered", typeof(T).FullName));
        }

        public static IJsonMapper<T> ThrowNoSuitableBuilderException<T>()
        {
            throw new JsonException(string.Format("Mapper for type '{0}' is not registered", typeof(T).FullName));
        }

        public static IJsonMapper<T> ThrowManyBuildersException<T>(List<IJsonMapperBuilder> builders)
        {
            var builderNames = string.Join(", ", builders.Select(b => b.GetType().FullName));
            throw new JsonException(string.Format("Type '{0}' can be built by many builders: [{1}]", typeof(T).FullName,
                builderNames));
        }

        public static T ThrowEndOfStreamException<T>()
        {
            throw new JsonException("End of stream");
        }

        public static T ThrowUnexpectedException<T>(Exception exc)
        {
            throw new JsonException("Unexpected exception", exc);
        }
    }
}