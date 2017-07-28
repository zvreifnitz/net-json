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
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Helper;
    using Parser;

    internal sealed class ObjectMapperBuilder<TClass> : IJsonMapperBuilder<TClass>
    {
        private readonly Dictionary<string, IJsonGetterSetter<TClass>> _getters =
            new Dictionary<string, IJsonGetterSetter<TClass>>();
        private readonly Dictionary<string, IJsonGetterSetter<TClass>> _setters =
            new Dictionary<string, IJsonGetterSetter<TClass>>();
        
        private Func<TClass> _instanceProvider;

        public IJsonMapperBuilder<TClass> NewInstanceProvider(
            Func<TClass> instanceProvider)
        {
            _instanceProvider = instanceProvider;
            return this;
        }

        public IJsonMapperBuilder<TClass> AddProperty<TProp>(
            Expression<Func<TClass, TProp>> property)
        {
            AddProperty(property, ExpressionHelper.GetPropertyName(property));
            return this;
        }

        public IJsonMapperBuilder<TClass> AddProperty<TProp>(
            Expression<Func<TClass, TProp>> property, string jsonPropName)
        {
            _getters.Add(jsonPropName.EncodeToJsonString(), JsonGetter<TClass, TProp>.Build(property));
            _setters.Add(jsonPropName, JsonSetter<TClass, TProp>.Build(property));
            return this;
        }

        public IJsonMapperBuilder<TClass> AddReadOnlyProperty<TProp>(
            Expression<Func<TClass, TProp>> property)
        {
            AddReadOnlyProperty(property, ExpressionHelper.GetPropertyName(property));
            return this;
        }

        public IJsonMapperBuilder<TClass> AddReadOnlyProperty<TProp>(
            Expression<Func<TClass, TProp>> property, string jsonPropName)
        {
            _getters.Add(jsonPropName.EncodeToJsonString(), JsonGetter<TClass, TProp>.Build(property));
            return this;
        }

        public IJsonMapperBuilder<TClass> AddWriteOnlyProperty<TProp>(
            Expression<Func<TClass, TProp>> property)
        {
            AddWriteOnlyProperty(property, ExpressionHelper.GetPropertyName(property));
            return this;
        }

        public IJsonMapperBuilder<TClass> AddWriteOnlyProperty<TProp>(
            Expression<Func<TClass, TProp>> property, string jsonPropName)
        {
            _setters.Add(jsonPropName, JsonSetter<TClass, TProp>.Build(property));
            return this;
        }

        public IJsonMapper<TClass> Build()
        {
            return new ObjectMapper<TClass>(
                _instanceProvider ?? ExpressionHelper.CreateDefaultConstructor<TClass>(),
                new Dictionary<string, IJsonGetterSetter<TClass>>(_getters),
                new Dictionary<string, IJsonGetterSetter<TClass>>(_setters));
        }
    }
}