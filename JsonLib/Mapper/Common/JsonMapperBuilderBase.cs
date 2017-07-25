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
    
    internal abstract class JsonMapperBuilderBase : IJsonMapperBuilder
    {
        private readonly MethodInfo _buildReflectionInvoke;

        protected JsonMapperBuilderBase()
        {
            _buildReflectionInvoke = GetType().GetRuntimeMethods().Single(mi => mi.Name == "BuildReflectionInvoke");
        }

        protected abstract Type GenericTypeDefinition { get; }

        protected abstract Type[] GetMethodTypes(Type type, Type[] types);

        protected virtual bool CheckTypes(Type[] types)
        {
            return true;
        }

        public bool CanBuild<T>(IJsonSerializators context)
        {
            var type = typeof(T);
            var genTypeDef = type.GetGenericTypeDefinition();
            if (genTypeDef != GenericTypeDefinition)
            {
                return false;
            }
            return CheckAllGenericParameters(context, type.GenericTypeArguments);
        }

        public IJsonMapper<T> Build<T>(IJsonSerializators context)
        {
            var type = typeof(T);
            var genTypeDef = type.GetGenericTypeDefinition();
            if (genTypeDef != GenericTypeDefinition)
            {
                return null;
            }
            var genericTypes = type.GenericTypeArguments;
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

        private bool CheckAllGenericParameters(IJsonSerializators context, Type[] types)
        {
            foreach (var type in types)
            {
                if (context.GetJsonSerializatorReflection(type) == null)
                {
                    return false;
                }
            }
            return CheckTypes(types);
        }

        private IJsonSerializator[] GetJsonSerializators(IJsonSerializators context, Type[] types)
        {
            IJsonSerializator[] result = new IJsonSerializator[types.Length];
            for (int i = 0; i < types.Length; i++)
            {
                IJsonSerializator tmp = context.GetJsonSerializatorReflection(types[i]);
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