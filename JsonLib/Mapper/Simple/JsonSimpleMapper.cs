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

namespace com.github.zvreifnitz.JsonLib.Mapper.Simple
{
    internal sealed class JsonSimpleMapper<T> : IJsonMapper<T>
    {
        private readonly ISimpleConverter<T> _converter;

        internal JsonSimpleMapper(ISimpleConverter<T> converter)
        {
            _converter = converter;
        }

        public bool CanSerialize => true;

        public bool CanDeserialize => true;

        public void Init(IJsonContext context)
        {
        }

        public void ToJson(IJsonContext context, IJsonWriter writer, T instance)
        {
            _converter.ToJson(context, writer, instance);
        }

        public T FromJson(IJsonContext context, IJsonReader reader)
        {
            _converter.FromJson(context, reader, out T result);
            return result;
        }
    }
}