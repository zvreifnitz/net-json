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

namespace com.github.zvreifnitz.JsonLib.Impl
{
    using Helper;
    using Converter;

    internal sealed class JsonMapper<T> : IJsonMapper<T>
    {
        private readonly IJsonConverter<T> _converter;

        internal JsonMapper(IJsonConverter<T> converter)
        {
            _converter = converter;
        }

        public bool CanSerialize => _converter.CanSerialize;

        public bool CanDeserialize => _converter.CanDeserialize;

        public void ToJson(IJsonSerializators context, IJsonWriter writer, T instance)
        {
            if (CanSerialize)
            {
                _converter.ToJson(context, writer, instance);
            }
            else
            {
                writer.WriteRaw("null");
            }
        }

        public T FromJson(IJsonSerializators context, IJsonReader reader)
        {
            if (CanDeserialize)
            {
                _converter.FromJson(context, reader, out T result);
                return result;
            }
            switch (reader.GetNextToken())
            {
                case JsonToken.ArrayStart:
                    ReadUntil(reader, JsonToken.ArrayStart, JsonToken.ArrayEnd);
                    return default(T);
                case JsonToken.ObjectStart:
                    ReadUntil(reader, JsonToken.ObjectStart, JsonToken.ObjectEnd);
                    return default(T);
                case JsonToken.False:
                case JsonToken.True:
                case JsonToken.Number:
                case JsonToken.String:
                    return default(T);
                default:
                    return ExceptionHelper.ThrowInvalidJsonException<T>();
            }
        }

        private static void ReadUntil(IJsonReader reader, JsonToken upToken, JsonToken downToken)
        {
            int count = 1;
            while (count > 0)
            {
                var token = reader.GetNextToken();
                if (token == downToken)
                {
                    count--;
                }
                else if (token == upToken)
                {
                    count++;
                }
            }
        }
    }
}