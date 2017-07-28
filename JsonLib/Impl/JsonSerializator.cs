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
    using System.IO;
    using System.Text;
    using Parser;

    internal sealed class JsonSerializator<T> : IJsonSerializator<T>
    {
        internal JsonSerializator(JsonContext context, IJsonMapper<T> mapper, IRuntimeMapperBuilder builder)
        {
            Context = context;
            Mapper = mapper;
            Builder = builder;
        }

        internal JsonContext Context { get; }

        public bool CanSerialize => Mapper.CanSerialize;

        public bool CanDeserialize => Mapper.CanDeserialize;

        public IRuntimeMapperBuilder Builder { get; }

        public IJsonMapper<T> Mapper { get; }

        public void ToJson(Stream stream, T instance)
        {
            ToJson(new StreamWriter(stream), instance);
        }

        public T FromJson(Stream stream)
        {
            return FromJson(new StreamReader(stream));
        }

        public void ToJson(TextWriter writer, T instance)
        {
            using (var jsonWriter = new JsonWriter(writer))
            {
                Mapper.ToJson(Context, jsonWriter, instance);
            }
        }

        public T FromJson(TextReader reader)
        {
            using (var jsonReader = new JsonReader(reader))
            {
                return Mapper.FromJson(Context, jsonReader);
            }
        }

        public void ToJson(StringBuilder builder, T instance)
        {
            using (var writer = new StringWriter(builder))
            {
                ToJson(writer, instance);
            }
        }

        public T FromJson(StringBuilder builder)
        {
            return FromJson(builder.ToString());
        }

        public string ToJson(T instance)
        {
            var sb = new StringBuilder();
            ToJson(sb, instance);
            return sb.ToString();
        }

        public T FromJson(string json)
        {
            using (var reader = new StringReader(json))
            {
                return FromJson(reader);
            }
        }
    }
}