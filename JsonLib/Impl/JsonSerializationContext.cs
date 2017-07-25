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
    using Helper;
    
    internal sealed class JsonSerializationContext : IJsonSerializationContext
    {
        private static int _IdSeed;

        private readonly object _lock = new object();
        private readonly int _id = Interlocked.Increment(ref _IdSeed);
        private readonly List<Action> _disposeActions = new List<Action>();
        private readonly List<IJsonMapperBuilder> _mapperBuilders = new List<IJsonMapperBuilder>();
        private bool _disposed;

        internal JsonSerializationContext()
        {
            this.RegisterDefaultMappers();
            this.RegisterDefaultBuilders();
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

        public bool RegisterMapperBulder(IJsonMapperBuilder builder)
        {
            if (builder == null)
            {
                return false;
            }
            lock (_lock)
            {
                if (_disposed)
                {
                    return false;
                }
                foreach (var existing in _mapperBuilders)
                {
                    if (existing == builder || existing.GetType() == builder.GetType())
                    {
                        return false;
                    }
                }
                _mapperBuilders.Add(builder);
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

        public bool UnregisterMapperBulder(IJsonMapperBuilder builder)
        {
            if (builder == null)
            {
                return false;
            }
            lock (_lock)
            {
                if (_disposed)
                {
                    return false;
                }
                IJsonMapperBuilder found = null;
                foreach (var existing in _mapperBuilders)
                {
                    if (existing == builder || existing.GetType() == builder.GetType())
                    {
                        found = existing;
                        break;
                    }
                }
                if (found == null)
                {
                    return false;
                }
                _mapperBuilders.Remove(found);
                return true;
            }
        }

        public IJsonSerializator<T> GetJsonSerializator<T>()
        {
            var wrapper = JsonSerializatorCache<T>.GetJsonSerializator(this);
            return wrapper != null ? wrapper.JsonSerializator : BuildJsonSerializator<T>();
        }

        private IJsonSerializator<T> BuildJsonSerializator<T>()
        {
            IJsonMapper<T> mapper = BuildJsonMapper<T>();
            RegisterMapper(mapper);
            var wrapper = JsonSerializatorCache<T>.GetJsonSerializator(this);
            return wrapper != null
                ? wrapper.JsonSerializator
                : ExceptionHelper.ThrowMapperNotRegisteredException<T>();
        }

        private IJsonMapper<T> BuildJsonMapper<T>()
        {
            List<IJsonMapperBuilder> builders = GetAvailableBuilders<T>();
            switch (builders.Count)
            {
                case 0:
                    return ExceptionHelper.ThrowNoSuitableBuilderException<T>();
                case 1:
                    return builders[0].Build<T>(this);
                default:
                    return ExceptionHelper.ThrowManyBuildersException<T>(builders);
            }
        }

        private List<IJsonMapperBuilder> GetAvailableBuilders<T>()
        {
            return GetAllBuilders().FindAll(b => b.CanBuild<T>(this));
        }

        private List<IJsonMapperBuilder> GetAllBuilders()
        {
            lock (_lock)
            {
                return new List<IJsonMapperBuilder>(_mapperBuilders);
            }
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
                _mapperBuilders.Clear();
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