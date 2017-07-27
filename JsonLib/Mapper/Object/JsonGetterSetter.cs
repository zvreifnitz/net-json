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

namespace com.github.zvreifnitz.JsonLib.Mapper.Object
{
    using System;
    using System.Linq.Expressions;
    using Helper;
    
    internal sealed class JsonGetterSetter<TClass, TProp> : IJsonGetterSetter<TClass>
    {
        private readonly Func<TClass, TProp> _getter;
        private readonly Action<TClass, TProp> _setter;
        private IJsonMapper<TProp> _jsonMapper;

        public JsonGetterSetter(Func<TClass, TProp> getter, Action<TClass, TProp> setter)
        {
            _getter = getter;
            _setter = setter;
        }

        internal static IJsonGetterSetter<TClass> Build(Expression<Func<TClass, TProp>> property)
        {
            return new JsonGetterSetter<TClass, TProp>(
                ExpressionHelper.CreateGetter(property),
                ExpressionHelper.CreateSetter(property));
        }
        
        public void Init(IJsonSerializators context)
        {
            _jsonMapper = context.GetJsonSerializator<TProp>().Mapper;
        }

        public void ToJson(IJsonSerializators context, IJsonWriter writer, TClass instance)
        {
            var value = _getter(instance);
            _jsonMapper.ToJson(context, writer, value);
        }

        public void FromJson(IJsonSerializators context, IJsonReader reader, TClass instance)
        {
            var value = _jsonMapper.FromJson(context, reader);
            _setter(instance, value);
        }
    }
}