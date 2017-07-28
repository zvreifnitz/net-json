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

namespace com.github.zvreifnitz.JsonLib.Mapper.Common
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Helper;

    internal abstract class DelegatingMapperBuilderBase : IRuntimeMapperBuilder
    {
        private readonly MethodInfo _buildReflectionInvoke;

        protected DelegatingMapperBuilderBase()
        {
            _buildReflectionInvoke = GetType().GetRuntimeMethods().Single(mi => mi.Name == "BuildReflectionInvoke");
        }

        protected abstract bool IsRequestedTypeSupported(Type type);

        protected abstract Type GenerateDelegatingType(Type type);

        protected abstract Type[] GetMethodTypes(Type type, Type delegatingType);

        public bool CanBuild<T>(IJsonContext context)
        {
            var type = typeof(T);
            if (!IsRequestedTypeSupported(type))
            {
                return false;
            }
            var delegatingType = GenerateDelegatingType(type);
            if (delegatingType == null)
            {
                return false;
            }
            return context.GetSerializatorReflection(delegatingType) != null;
        }

        public IJsonMapper<T> Build<T>(IJsonContext context)
        {
            var type = typeof(T);
            if (!IsRequestedTypeSupported(type))
            {
                return null;
            }
            var delegatingType = GenerateDelegatingType(type);
            if (delegatingType == null)
            {
                return null;
            }
            var serializator = context.GetSerializatorReflection(delegatingType);
            if (serializator == null)
            {
                return null;
            }
            return Build<T>(type, delegatingType, serializator);
        }

        private IJsonMapper<T> Build<T>(Type type, Type delegatingType, IJsonSerializator jsonSerializator)
        {
            try
            {
                Type[] methodTypes = GetMethodTypes(type, delegatingType);
                MethodInfo generic = _buildReflectionInvoke.MakeGenericMethod(methodTypes);
                object result = generic.Invoke(this, new object[] {jsonSerializator});
                return (IJsonMapper<T>)result;
            }
            catch (Exception exc)
            {
                return ExceptionHelper.ThrowUnexpectedException<IJsonMapper<T>>(exc);
            }
        }
    }
}