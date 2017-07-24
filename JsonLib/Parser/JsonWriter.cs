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

namespace com.github.zvreifnitz.JsonLib.Parser
{
    using System.IO;

    internal sealed class JsonWriter : IJsonWriter
    {
        private readonly TextWriter writer;

        public JsonWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        public void EncodeAndWrite(string value)
        {
            WriteRaw(value.EncodeToJsonString());
        }

        public void WriteRaw(string value)
        {
            writer.Write(value);
        }
    }
}