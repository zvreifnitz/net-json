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
    using System;
    using System.Threading;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    internal sealed class JsonSerializationContext : IJsonSerializationContext
    {
        private static int IdSeed;

        private readonly object _lock = new object();
        private readonly int _id = Interlocked.Increment(ref IdSeed);
        private readonly List<Action> _disposeActions = new List<Action>();
        private bool _disposed;

        internal JsonSerializationContext()
        {
            this.RegisterDefaultMappers();
        }

        public bool RegisterMapper<T>(IJsonMapper<T> mapper)
        {
            if (mapper == null)
            {
                return false;
            }
            lock (_lock)
            {
                if (_disposed)
                {
                    return false;
                }
                JsonSerializatorCache<T>.Register(this, mapper);
                _disposeActions.Add(() => UnregisterMapperInternal(mapper));
                return true;
            }
        }

        public bool UnregisterMapper<T>(IJsonMapper<T> mapper)
        {
            if (mapper == null)
            {
                return false;
            }
            lock (_lock)
            {
                return !_disposed && UnregisterMapperInternal(mapper);
            }
        }

        public IJsonSerializator<T> GetJsonSerializator<T>()
        {
            var wrapper = JsonSerializatorCache<T>.GetJsonSerializator(this);
            return wrapper != null
                ? wrapper.JsonSerializator
                : throw new JsonException(string.Format("Mapper for type '{0}' is not registered", typeof(T).FullName));
        }

        private bool UnregisterMapperInternal<T>(IJsonMapper<T> mapper)
        {
            var wrapper = JsonSerializatorCache<T>.GetJsonSerializator(this);
            if (wrapper == null || wrapper.Mapper != mapper && wrapper.MapperType != mapper.GetType())
            {
                return false;
            }
            JsonSerializatorCache<T>.Unregister(this);
            return true;
        }

        public void Dispose()
        {
            List<Action> actions;
            lock (_lock)
            {
                _disposed = true;
                actions = new List<Action>(_disposeActions);
                _disposeActions.Clear();
            }
            actions.ForEach(a => a());
        }

        private sealed class Wrapper<T>
        {
            internal readonly IJsonSerializator<T> JsonSerializator;
            internal readonly IJsonMapper<T> Mapper;
            internal readonly Type MapperType;

            public Wrapper(IJsonSerializationContext context, IJsonMapper<T> mapper)
            {
                Mapper = mapper;
                JsonSerializator = new JsonSerializator<T>(context, mapper);
                MapperType = mapper.GetType();
            }
        }

        private static class JsonSerializatorCache<T>
        {
            private static readonly ConcurrentDictionary<int, Wrapper<T>> Cache =
                new ConcurrentDictionary<int, Wrapper<T>>();

            internal static Wrapper<T> GetJsonSerializator(JsonSerializationContext context)
            {
                return Cache.TryGetValue(context._id, out Wrapper<T> wrapper)
                    ? wrapper
                    : null;
            }

            internal static void Register(JsonSerializationContext context, IJsonMapper<T> mapper)
            {
                Cache[context._id] = new Wrapper<T>(context, mapper);
            }

            internal static void Unregister(JsonSerializationContext context)
            {
                Cache.TryRemove(context._id, out Wrapper<T> wrapper);
            }
        }
    }
}