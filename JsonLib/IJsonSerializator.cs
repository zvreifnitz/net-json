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

namespace com.github.zvreifnitz.JsonLib
{
    using System.IO;
    using System.Text;

    public interface IJsonSerializator
    {
    }

    public interface IJsonSerializator<T> : IJsonSerializator
    {
        IJsonMapper<T> Mapper { get; }

        void ToJson(Stream stream, T instance);
        T FromJson(Stream stream);

        void ToJson(TextWriter writer, T instance);
        T FromJson(TextReader reader);

        void ToJson(StringBuilder builder, T instance);
        T FromJson(StringBuilder builder);

        string ToJson(T instance);
        T FromJson(string json);
    }
}