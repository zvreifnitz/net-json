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

namespace JsonLibExample
{
    using com.github.zvreifnitz.JsonLib;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = JsonLibFactory.NewContextBuilder();
            var simpleObjectMapper = JsonLibFactory.NewMapperBuilder<SimpleObject>()
                .NewInstanceProvider(() => new SimpleObject())
                .AddProperty(o => o.IntValue)
                .AddProperty(o => o.GuidValue)
                .AddProperty(o => o.StringValue)
                .Build();
            builder.RegisterMapper(simpleObjectMapper);
            var mediumObjectMapper = JsonLibFactory.NewMapperBuilder<MediumObject>()
                .NewInstanceProvider(() => new MediumObject())
                .AddProperty(o => o.Name)
                .AddProperty(o => o.MaybeInt)
                .AddProperty(o => o.SimpleObjectList)
                .AddProperty(o => o.SimpleObjectDictionary)
                .AddProperty(o => o.LongSet)
                .AddProperty(o => o.IntArray)
                .Build();
            builder.RegisterMapper(mediumObjectMapper);
            
            using (var ctx = builder.Build())
            {
                BuiltInTypes.Run(ctx);
                for (int i = 0; i < 10; i++)
                {
                    new StringPerfComparison().Run(ctx);
                    new DictionaryPerfComparison().Run(ctx);
                    new SimpleObjectPerfComparison().Run(ctx);
                    new MediumObjectPerfComparison().Run(ctx);
                }
            }
        }
    }
}