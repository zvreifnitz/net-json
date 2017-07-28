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

    internal abstract class MapperBuilderBase : IRuntimeMapperBuilder
    {
        private readonly MethodInfo _buildReflectionInvoke;

        protected MapperBuilderBase()
        {
            _buildReflectionInvoke = GetType().GetRuntimeMethods().Single(mi => mi.Name == "BuildReflectionInvoke");
        }

        protected abstract bool IsRequestedTypeSupported(Type type);

        protected abstract Type[] GetMethodTypes(Type type, Type[] types);

        protected virtual bool CheckGenericTypeArgs(Type[] types)
        {
            return true;
        }

        public bool CanBuild<T>(IJsonContext context)
        {
            var type = typeof(T);
            if (!IsRequestedTypeSupported(type))
            {
                return false;
            }
            var genericTypes = type.GenericTypeArguments;
            if (!CheckGenericTypeArgs(genericTypes))
            {
                return false;
            }
            return CheckAllGenericParameters(context, genericTypes);
        }

        public IJsonMapper<T> Build<T>(IJsonContext context)
        {
            var type = typeof(T);
            if (!IsRequestedTypeSupported(type))
            {
                return null;
            }
            var genericTypes = type.GenericTypeArguments;
            if (!CheckGenericTypeArgs(genericTypes))
            {
                return null;
            }
            var serializators = GetJsonSerializators(context, genericTypes);
            if (serializators == null)
            {
                return null;
            }
            return Build<T>(type, genericTypes, serializators);
        }

        private IJsonMapper<T> Build<T>(Type type, Type[] types, IJsonSerializator[] jsonSerializators)
        {
            try
            {
                Type[] methodTypes = GetMethodTypes(type, types);
                MethodInfo generic = _buildReflectionInvoke.MakeGenericMethod(methodTypes);
                object result = generic.Invoke(this, jsonSerializators);
                return (IJsonMapper<T>)result;
            }
            catch (Exception exc)
            {
                return ExceptionHelper.ThrowUnexpectedException<IJsonMapper<T>>(exc);
            }
        }

        private bool CheckAllGenericParameters(IJsonContext context, Type[] types)
        {
            foreach (var type in types)
            {
                if (context.GetSerializatorReflection(type) == null)
                {
                    return false;
                }
            }
            return true;
        }

        private IJsonSerializator[] GetJsonSerializators(IJsonContext context, Type[] types)
        {
            IJsonSerializator[] result = new IJsonSerializator[types.Length];
            for (int i = 0; i < types.Length; i++)
            {
                IJsonSerializator tmp = context.GetSerializatorReflection(types[i]);
                if (tmp == null)
                {
                    return null;
                }
                result[i] = tmp;
            }
            return result;
        }
    }
}