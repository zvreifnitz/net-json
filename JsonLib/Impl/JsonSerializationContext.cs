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
    using System.Linq;
    using System.Threading;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Helper;

    internal sealed class JsonSerializationContext : IJsonSerializationContext
    {
        private static int _idSeed;

        private readonly object _lock = new object();
        private readonly int _id = Interlocked.Increment(ref _idSeed);
        private readonly List<IWrapper> _mapperWrappers = new List<IWrapper>();
        private readonly List<IWrapper> _mapperBuilders = new List<IWrapper>();
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
                UnregisterMapperSync<T>();
                MapperWrapper<T> wrapper = new MapperWrapper<T>(this, mapper);
                JsonSerializatorCache<T>.Register(wrapper);
                _mapperWrappers.Add(wrapper);
                return true;
            }
        }

        public bool RegisterMapperBulder<T>(T builder) where T : IJsonMapperBuilder
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
                UnregisterMapperBulderSync<T>();
                BuilderWrapper<T> wrapper = new BuilderWrapper<T>(this, builder);
                _mapperBuilders.Add(wrapper);
                return true;
            }
        }

        public bool UnregisterMapper<T>()
        {
            lock (_lock)
            {
                return !_disposed && UnregisterMapperSync<T>();
            }
        }

        public bool UnregisterMapperBulder<T>() where T : IJsonMapperBuilder
        {
            lock (_lock)
            {
                return !_disposed && UnregisterMapperBulderSync<T>();
            }
        }

        public IJsonSerializator<T> GetJsonSerializator<T>()
        {
            var wrapper = JsonSerializatorCache<T>.GetJsonSerializator(this);
            if (wrapper != null)
            {
                return wrapper.JsonSerializator;
            }
            wrapper = BuildJsonSerializator<T>();
            if (wrapper != null)
            {
                return wrapper.JsonSerializator;
            }
            return ExceptionHelper.ThrowMapperNotRegisteredException<T>();
        }

        public IJsonSerializationContext Clone()
        {
            IJsonSerializationContext result = new JsonSerializationContext();
            lock (_lock)
            {
                foreach (var wrapper in _mapperWrappers)
                {
                    wrapper.Clone(result);
                }
                foreach (var builder in _mapperBuilders)
                {
                    builder.Clone(result);
                }
            }
            return result;
        }

        private MapperWrapper<T> BuildJsonSerializator<T>()
        {
            lock (_lock)
            {
                var existingWrapper = JsonSerializatorCache<T>.GetJsonSerializator(this);
                if (existingWrapper != null)
                {
                    return existingWrapper;
                }
                MapperWrapper<T> createdWrapper = BuildMapperWrapperSync<T>();
                JsonSerializatorCache<T>.Register(createdWrapper);
                _mapperWrappers.Add(createdWrapper);
                return createdWrapper;
            }
        }

        private MapperWrapper<T> BuildMapperWrapperSync<T>()
        {
            IJsonMapperBuilder builder = GetBuilderSync<T>();
            IJsonMapper<T> mapper = builder.Build<T>(this);
            return new MapperWrapper<T>(this, mapper, builder);
        }

        private IJsonMapperBuilder GetBuilderSync<T>()
        {
            List<IJsonMapperBuilder> builders = GetAvailableBuildersSync<T>();
            switch (builders.Count)
            {
                case 0:
                    return ExceptionHelper.ThrowNoSuitableBuilderException<T>();
                case 1:
                    return builders[0];
                default:
                    return ExceptionHelper.ThrowManyBuildersException<T>(builders);
            }
        }

        private List<IJsonMapperBuilder> GetAvailableBuildersSync<T>()
        {
            return GetAllBuildersSync().FindAll(b => b.CanBuild<T>(this));
        }

        private List<IJsonMapperBuilder> GetAllBuildersSync()
        {
            lock (_lock)
            {
                return new List<IJsonMapperBuilder>(_mapperBuilders.Select(i => i.GetBuilder()));
            }
        }

        private bool UnregisterMapperSync<T>()
        {
            var wrapper = JsonSerializatorCache<T>.Unregister(this);
            if (wrapper == null)
            {
                return false;
            }
            _mapperWrappers.Remove(wrapper);
            return true;
        }

        private bool UnregisterMapperBulderSync<T>()
        {
            var type = typeof(T);
            IJsonMapperBuilder found = null;
            foreach (var builder in _mapperBuilders)
            {
                if (builder.GetBuilder().GetType() == type)
                {
                    found = builder.GetBuilder();
                    break;
                }
            }
            if (found == null)
            {
                return false;
            }
            var toBeDisposed = new List<IWrapper>();
            foreach (var mapperWrapper in _mapperWrappers)
            {
                var builder = mapperWrapper.GetBuilder();
                if (builder == found)
                {
                    toBeDisposed.Add(mapperWrapper);
                }
            }
            toBeDisposed.ForEach(a => Dispose());
            return true;
        }

        public void Dispose()
        {
            List<IWrapper> toBeDisposed;
            lock (_lock)
            {
                _disposed = true;
                toBeDisposed = new List<IWrapper>(_mapperWrappers);
                _mapperWrappers.Clear();
                _mapperBuilders.Clear();
            }
            toBeDisposed.ForEach(a => Dispose());
        }

        private interface IWrapper
        {
            IJsonMapperBuilder GetBuilder();
            void Clone(IJsonSerializationContext context);
            void Dispose();
            int ContextId { get; }
        }

        private sealed class BuilderWrapper<T> : IWrapper where T : IJsonMapperBuilder
        {
            private readonly JsonSerializationContext _context;
            private readonly T _builder;

            internal BuilderWrapper(JsonSerializationContext context, T builder)
            {
                _context = context;
                _builder = builder;
            }

            public IJsonMapperBuilder GetBuilder()
            {
                return _builder;
            }

            public void Clone(IJsonSerializationContext context)
            {
                context.RegisterMapperBulder(_builder);
            }

            public void Dispose()
            {
                _context.UnregisterMapperBulder<T>();
            }

            public int ContextId => _context._id;
        }

        private sealed class MapperWrapper<T> : IWrapper
        {
            internal readonly IJsonSerializator<T> JsonSerializator;
            private readonly JsonSerializationContext _context;
            private readonly IJsonMapper<T> _mapper;
            private readonly IJsonMapperBuilder _builder;

            internal MapperWrapper(JsonSerializationContext context, IJsonMapper<T> mapper,
                IJsonMapperBuilder builder = null)
            {
                _builder = builder;
                _mapper = mapper;
                _context = context;
                JsonSerializator = new JsonSerializator<T>(context, mapper);
            }

            public IJsonMapperBuilder GetBuilder()
            {
                return _builder;
            }

            public void Clone(IJsonSerializationContext context)
            {
                context.RegisterMapper(_mapper);
            }

            public void Dispose()
            {
                _context.UnregisterMapper<T>();
            }

            public int ContextId => _context._id;
        }

        private static class JsonSerializatorCache<T>
        {
            private static readonly ConcurrentDictionary<int, MapperWrapper<T>> Cache =
                new ConcurrentDictionary<int, MapperWrapper<T>>();

            internal static MapperWrapper<T> GetJsonSerializator(JsonSerializationContext context)
            {
                return Cache.TryGetValue(context._id, out MapperWrapper<T> wrapper) ? wrapper : null;
            }

            internal static void Register(MapperWrapper<T> wrapper)
            {
                Cache[wrapper.ContextId] = wrapper;
            }

            internal static MapperWrapper<T> Unregister(JsonSerializationContext context)
            {
                return Cache.TryRemove(context._id, out MapperWrapper<T> wrapper) ? wrapper : null;
            }
        }
    }
}