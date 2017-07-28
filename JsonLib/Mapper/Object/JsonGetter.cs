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

    internal sealed class JsonGetter<TClass, TProp> : IJsonGetterSetter<TClass>
    {
        private readonly Func<TClass, TProp> _getter;
        private IJsonMapper<TProp> _jsonMapper;

        private JsonGetter(Func<TClass, TProp> getter)
        {
            _getter = getter;
        }

        internal static IJsonGetterSetter<TClass> Build(Expression<Func<TClass, TProp>> property)
        {
            return new JsonGetter<TClass, TProp>(ExpressionHelper.CreateGetter(property));
        }

        public void Init(IJsonContext context)
        {
            _jsonMapper = context.GetSerializator<TProp>().Mapper;
        }

        public void ToJson(IJsonContext context, IJsonWriter writer, TClass instance)
        {
            var value = _getter(instance);
            _jsonMapper.ToJson(context, writer, value);
        }

        public void FromJson(IJsonContext context, IJsonReader reader, TClass instance)
        {
        }
    }
}