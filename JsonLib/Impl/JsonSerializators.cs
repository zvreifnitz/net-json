using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using com.github.zvreifnitz.JsonLib.Helper;

namespace com.github.zvreifnitz.JsonLib.Impl
{
    internal sealed class JsonSerializators : IJsonSerializators
    {
        private static int _idSeed;

        private readonly object _lock = new object();
        private readonly int _id = Interlocked.Increment(ref _idSeed);
        private readonly List<IWrapper> _mapperWrappers = new List<IWrapper>();
        private readonly List<IWrapper> _mapperBuilders = new List<IWrapper>();
        private bool _disposed;
        private bool _frozen;

        internal bool RegisterMapper<T>(IJsonMapper<T> mapper)
        {
            if (mapper == null)
            {
                return false;
            }
            lock (_lock)
            {
                if (_frozen || _disposed)
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

        internal bool RegisterMapperBulder<T>(T builder) where T : IJsonMapperBuilder
        {
            if (builder == null)
            {
                return false;
            }
            lock (_lock)
            {
                if (_frozen || _disposed)
                {
                    return false;
                }
                UnregisterMapperBulderSync<T>();
                BuilderWrapper<T> wrapper = new BuilderWrapper<T>(this, builder);
                _mapperBuilders.Add(wrapper);
                return true;
            }
        }

        internal bool UnregisterMapper<T>()
        {
            lock (_lock)
            {
                return !_frozen && !_disposed && UnregisterMapperSync<T>();
            }
        }

        internal bool UnregisterMapperBulder<T>() where T : IJsonMapperBuilder
        {
            lock (_lock)
            {
                return !_frozen && !_disposed && UnregisterMapperBulderSync<T>();
            }
        }

        internal void Freeze()
        {
            lock (_lock)
            {
                _frozen = true;
            }
            _mapperWrappers.ForEach(w => w.Init());
        }

        public bool TryGetJsonSerializator<T>(out IJsonSerializator<T> serializator)
        {
            try
            {
                serializator = GetJsonSerializator<T>();
                return true;
            }
            catch (JsonException)
            {
                serializator = null;
                return false;
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
            mapper.Init(this);
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
            return new List<IJsonMapperBuilder>(_mapperBuilders.Select(i => i.GetBuilder()));
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
            void Clone(IJsonSerializatorsBuilder context);
            void Init();
            void Dispose();
            int ContextId { get; }
        }

        private sealed class BuilderWrapper<T> : IWrapper where T : IJsonMapperBuilder
        {
            private readonly JsonSerializators _serializators;
            private readonly T _builder;

            internal BuilderWrapper(JsonSerializators serializators, T builder)
            {
                _serializators = serializators;
                _builder = builder;
            }

            public IJsonMapperBuilder GetBuilder()
            {
                return _builder;
            }

            public void Init()
            {
            }

            public void Clone(IJsonSerializatorsBuilder context)
            {
                context.RegisterMapperBulder(_builder);
            }

            public void Dispose()
            {
                _serializators.UnregisterMapperBulder<T>();
            }

            public int ContextId => _serializators._id;
        }

        private sealed class MapperWrapper<T> : IWrapper
        {
            internal readonly IJsonSerializator<T> JsonSerializator;
            private readonly JsonSerializators _serializators;
            private readonly IJsonMapper<T> _mapper;
            private readonly IJsonMapperBuilder _builder;

            internal MapperWrapper(JsonSerializators serializators, IJsonMapper<T> mapper,
                IJsonMapperBuilder builder = null)
            {
                _builder = builder;
                _mapper = mapper;
                _serializators = serializators;
                JsonSerializator = new JsonSerializator<T>(_serializators, mapper);
            }

            public IJsonMapperBuilder GetBuilder()
            {
                return _builder;
            }

            public void Init()
            {
                _mapper.Init(_serializators);
            }

            public void Clone(IJsonSerializatorsBuilder context)
            {
                context.RegisterMapper(_mapper);
            }

            public void Dispose()
            {
                _serializators.UnregisterMapper<T>();
            }

            public int ContextId => _serializators._id;
        }

        private static class JsonSerializatorCache<T>
        {
            private static readonly ConcurrentDictionary<int, MapperWrapper<T>> Cache =
                new ConcurrentDictionary<int, MapperWrapper<T>>();

            internal static MapperWrapper<T> GetJsonSerializator(JsonSerializators context)
            {
                return Cache.TryGetValue(context._id, out MapperWrapper<T> wrapper) ? wrapper : null;
            }

            internal static void Register(MapperWrapper<T> wrapper)
            {
                Cache[wrapper.ContextId] = wrapper;
            }

            internal static MapperWrapper<T> Unregister(JsonSerializators context)
            {
                return Cache.TryRemove(context._id, out MapperWrapper<T> wrapper) ? wrapper : null;
            }
        }
    }
}