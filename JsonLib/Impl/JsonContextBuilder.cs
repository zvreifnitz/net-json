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
    internal sealed class JsonContextBuilder : IJsonContextBuilder
    {
        private readonly JsonContext _context = new JsonContext();

        public JsonContextBuilder()
        {
            this.RegisterDefaultMappers();
            this.RegisterDefaultBuilders();
        }

        public bool RegisterMapper<T>(IJsonMapper<T> mapper)
        {
            return _context.RegisterMapper(mapper);
        }

        public bool UnregisterMapper<T>()
        {
            return _context.UnregisterMapper<T>();
        }

        public bool RegisterBuilder<T>(T builder) where T : IRuntimeMapperBuilder
        {
            return _context.RegisterMapperBulder(builder);
        }

        public bool UnregisterBuilder<T>() where T : IRuntimeMapperBuilder
        {
            return _context.UnregisterMapperBulder<T>();
        }

        public IJsonContext Build()
        {
            _context.Freeze();
            return _context;
        }
    }
}