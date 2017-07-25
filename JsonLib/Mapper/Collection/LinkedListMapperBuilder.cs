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

namespace com.github.zvreifnitz.JsonLib.Mapper.Collection
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Helper;
    using Parser;
    
    internal sealed class LinkedListMapperBuilder : IJsonMapperBuilder
    {
        private static readonly MethodInfo CreateMapperMethod =
            typeof(LinkedListMapperBuilder).GetRuntimeMethod("CreateMapper", new[] {typeof(IJsonSerializators)});

        private static readonly Type ListType = typeof(LinkedList<>);

        public bool CanBuild<T>(IJsonSerializators context)
        {
            var type = typeof(T);
            if (type.GenericTypeArguments.Length != 1)
            {
                return false;
            }
            var genType = type.GetGenericTypeDefinition();
            if (genType != ListType)
            {
                return false;
            }
            return context.GetJsonSerializatorReflection(type.GenericTypeArguments[0]) != null;
        }

        public IJsonMapper<T> Build<T>(IJsonSerializators context)
        {
            var type = typeof(T);
            if (type.GenericTypeArguments.Length != 1)
            {
                return null;
            }
            var genType = type.GetGenericTypeDefinition();
            if (genType != ListType)
            {
                return null;
            }
            return (IJsonMapper<T>)CreateMapper(type.GenericTypeArguments[0], context);
        }

        private IJsonMapper CreateMapper(Type type, IJsonSerializators context)
        {
            try
            {
                MethodInfo generic = CreateMapperMethod.MakeGenericMethod(type);
                object result = generic.Invoke(this, new object[] {context});
                return (IJsonMapper)result;
            }
            catch (Exception exc)
            {
                return ExceptionHelper.ThrowUnexpectedException<IJsonMapper>(exc);
            }
        }

        public IJsonMapper<LinkedList<T>> CreateMapper<T>(IJsonSerializators context)
        {
            try
            {
                IJsonSerializator<T> serializator = context.GetJsonSerializator<T>();
                return new ListMapper<T>(serializator);
            }
            catch (Exception exc)
            {
                return ExceptionHelper.ThrowUnexpectedException<IJsonMapper<LinkedList<T>>>(exc);
            }
        }

        private sealed class ListMapper<T> : IJsonMapper<LinkedList<T>>
        {
            private readonly IJsonSerializator<T> _serializator;

            public ListMapper(IJsonSerializator<T> serializator)
            {
                _serializator = serializator;
            }

            public bool CanSerialize => true;

            public bool CanDeserialize => true;

            public void ToJson(IJsonSerializators context, IJsonWriter writer, LinkedList<T> instance)
            {
                if (instance == null)
                {
                    writer.WriteRaw(JsonLiterals.Null);
                }
                else
                {
                    writer.WriteRaw(JsonLiterals.ArrayStart);
                    if (instance.Count > 0)
                    {
                        bool addComma = false;
                        foreach (var item in instance)
                        {
                            if (addComma)
                            {
                                writer.WriteRaw(JsonLiterals.Comma);
                            }
                            else
                            {
                                addComma = true;
                            }
                            ToJsonItem(context, writer, item);
                        }
                    }
                    writer.WriteRaw(JsonLiterals.ArrayEnd);
                }
            }

            private void ToJsonItem(IJsonSerializators context, IJsonWriter writer, T instance)
            {
                if (_serializator.Mapper.CanSerialize)
                {
                    _serializator.Mapper.ToJson(context, writer, instance);
                }
                else
                {
                    writer.WriteRaw(JsonLiterals.Null);
                }
            }

            public LinkedList<T> FromJson(IJsonSerializators context, IJsonReader reader)
            {
                JsonToken token = reader.GetNextToken();
                if (token == JsonToken.Null)
                {
                    return null;
                }
                if (token != JsonToken.ArrayStart)
                {
                    return ExceptionHelper.ThrowInvalidJsonException<LinkedList<T>>();
                }
                LinkedList<T> result = new LinkedList<T>();
                while (true)
                {
                    result.AddLast(FromJsonItem(context, reader));
                    token = reader.GetNextToken();
                    if (token == JsonToken.ArrayEnd)
                    {
                        break;
                    }
                    if (token != JsonToken.Comma)
                    {
                        return ExceptionHelper.ThrowInvalidJsonException<LinkedList<T>>();
                    }
                }
                return result;
            }

            private T FromJsonItem(IJsonSerializators context, IJsonReader reader)
            {
                if (_serializator.Mapper.CanDeserialize)
                {
                    return _serializator.Mapper.FromJson(context, reader);
                }
                return default(T);
            }
        }
    }
}