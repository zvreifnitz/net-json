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
    using System.Reflection;

    internal static class JsonSerializatorsHelper
    {
        private static readonly MethodInfo GetJsonSerializatorMethod =
            typeof(IJsonSerializators).GetRuntimeMethod("GetJsonSerializator", new Type[0]);

        public static void ThrowIfNotMatch(IJsonReader reader, JsonToken token)
        {
            if (reader.GetNextToken() != token)
            {
                ExceptionHelper.ThrowInvalidJsonException<object>();
            }
        }
        
        public static IJsonSerializator GetJsonSerializatorReflection(this IJsonSerializators serializators, Type type)
        {
            try
            {
                MethodInfo generic = GetJsonSerializatorMethod.MakeGenericMethod(type);
                object result = generic.Invoke(serializators, null);
                return (IJsonSerializator)result;
            }
            catch (JsonException exc)
            {
                if (exc.Type == JsonExceptionType.MapperNotRegistered)
                {
                    return null;
                }
                throw;
            }
            catch (Exception exc)
            {
                return ExceptionHelper.ThrowUnexpectedException<IJsonSerializator>(exc);
            }
        }
    }
}