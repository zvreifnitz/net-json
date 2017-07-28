﻿/*
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
    using Newtonsoft.Json;
    using System;
    using com.github.zvreifnitz.JsonLib;
    using System.Collections.Generic;

    public class DictionaryPerfComparison : PerfComparisonBase
    {
        private static readonly Dictionary<string, Dictionary<string, Dictionary<string, int?>>> CleanValue =
            BuildCleanValue();

        private static Dictionary<string, Dictionary<string, Dictionary<string, int?>>> BuildCleanValue()
        {
            var result = new Dictionary<string, Dictionary<string, Dictionary<string, int?>>>(5);
            for (int i = 0; i < 5; i++)
            {
                var intermediate = new Dictionary<string, Dictionary<string, int?>>(5);
                for (int j = 0; j < 5; j++)
                {
                    var last = new Dictionary<string, int?>(5);
                    for (int k = 0; k < 5; k++)
                    {
                        last.Add(k.ToString(), k % 3 == 0 ? (int?)null : k);
                    }
                    intermediate.Add(j.ToString(), last);
                }
                intermediate.Add("5", new Dictionary<string, int?>());
                intermediate.Add("6", null);
                result.Add(i.ToString(), intermediate);
            }
            result.Add("5", null);
            result.Add("6", new Dictionary<string, Dictionary<string, int?>>());
            return result;
        }

        private const string JsonValue =
                "{\"0\":{\"0\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"1\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"2\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"3\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"4\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"5\":{},\"6\":null},\"1\":{\"0\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"1\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"2\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"3\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"4\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"5\":{},\"6\":null},\"2\":{\"0\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"1\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"2\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"3\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"4\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"5\":{},\"6\":null},\"3\":{\"0\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"1\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"2\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"3\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"4\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"5\":{},\"6\":null},\"4\":{\"0\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"1\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"2\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"3\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"4\":{\"0\":null,\"1\":1,\"2\":2,\"3\":null,\"4\":4},\"5\":{},\"6\":null},\"5\":null,\"6\":{}}"
            ;

        public override void Run(IJsonContext ctx)
        {
            long serNewtonsoftJson = 0L;
            string serNewtonsoftJsonExample = null;
            long serJsonLib = 0L;
            string serJsonLibExample = null;
            for (int i = 0; i < RepetitionCount; i++)
            {
                var njResult = SerializeTimeNewtonsoftJson();
                serNewtonsoftJson += njResult.Item1;
                serNewtonsoftJsonExample = njResult.Item2;
                var jlResult = SerializeTimeJsonLib(ctx);
                serJsonLib += jlResult.Item1;
                serJsonLibExample = jlResult.Item2;
            }

            long deserNewtonsoftJson = 0L;
            Dictionary<string, Dictionary<string, Dictionary<string, int?>>> deserNewtonsoftJsonExample = null;
            long deserJsonLib = 0L;
            Dictionary<string, Dictionary<string, Dictionary<string, int?>>> deserJsonLibExample = null;
            for (int i = 0; i < RepetitionCount; i++)
            {
                var njResult = DeserializeTimeNewtonsoftJson();
                deserNewtonsoftJson += njResult.Item1;
                deserNewtonsoftJsonExample = njResult.Item2;
                var jlResult = DeserializeTimeJsonLib(ctx);
                deserJsonLib += jlResult.Item1;
                deserJsonLibExample = jlResult.Item2;
            }

            long totalNewtonsoftJson = serNewtonsoftJson + deserNewtonsoftJson;
            long totalJsonLib = serJsonLib + deserJsonLib;
            double totalItems = RepetitionCount * TestCount;

            Console.Out.WriteLine("################################################");
            Console.Out.WriteLine("Dictionary perf comparison");
            Console.Out.WriteLine("################################################");
            Console.Out.WriteLine("Newtonsoft.Json");
            Console.Out.WriteLine("================================================");
            Console.Out.WriteLine("  Serialize: {0}, AvgTime: {1} ms, Example: {2}",
                serNewtonsoftJson, TimeSpan.FromTicks(serNewtonsoftJson).TotalMilliseconds / totalItems,
                serNewtonsoftJsonExample);
            Console.Out.WriteLine("Deserialize: {0}, AvgTime: {1} ms, Example: {2}",
                deserNewtonsoftJson, TimeSpan.FromTicks(deserNewtonsoftJson).TotalMilliseconds / totalItems,
                deserNewtonsoftJsonExample);
            Console.Out.WriteLine("      Total: {0}", totalNewtonsoftJson);
            Console.Out.WriteLine("################################################");
            Console.Out.WriteLine("JsonLib");
            Console.Out.WriteLine("================================================");
            Console.Out.WriteLine("  Serialize: {0}, AvgTime: {1} ms, Example: {2}",
                serJsonLib, TimeSpan.FromTicks(serJsonLib).TotalMilliseconds / totalItems, serJsonLibExample);
            Console.Out.WriteLine("Deserialize: {0}, AvgTime: {1} ms, Example: {2}",
                deserJsonLib, TimeSpan.FromTicks(deserJsonLib).TotalMilliseconds / totalItems, deserJsonLibExample);
            Console.Out.WriteLine("      Total: {0}", totalJsonLib);
            Console.Out.WriteLine("################################################");
            Console.Out.WriteLine("Ratio");
            Console.Out.WriteLine("================================================");
            Console.Out.WriteLine("  Serialize: {0}", (double)serNewtonsoftJson / serJsonLib);
            Console.Out.WriteLine("Deserialize: {0}", (double)deserNewtonsoftJson / deserJsonLib);
            Console.Out.WriteLine("      Total: {0}", (double)totalNewtonsoftJson / totalJsonLib);
            Console.Out.WriteLine();
        }

        private Tuple<long, string> SerializeTimeNewtonsoftJson()
        {
            SerializeDelegate<Dictionary<string, Dictionary<string, Dictionary<string, int?>>>> serializer =
                JsonConvert.SerializeObject;
            return SerializeTime(serializer, CleanValue);
        }

        private Tuple<long, Dictionary<string, Dictionary<string, Dictionary<string, int?>>>>
            DeserializeTimeNewtonsoftJson()
        {
            DeserializeDelegate<Dictionary<string, Dictionary<string, Dictionary<string, int?>>>> deserializer =
                JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, int?>>>>;
            return DeserializeTime(deserializer, JsonValue);
        }

        private Tuple<long, string> SerializeTimeJsonLib(IJsonContext ctx)
        {
            SerializeDelegate<Dictionary<string, Dictionary<string, Dictionary<string, int?>>>> serializer =
                ctx.GetSerializator<Dictionary<string, Dictionary<string, Dictionary<string, int?>>>>().ToJson;
            return SerializeTime(serializer, CleanValue);
        }

        private Tuple<long, Dictionary<string, Dictionary<string, Dictionary<string, int?>>>> DeserializeTimeJsonLib(
            IJsonContext ctx)
        {
            DeserializeDelegate<Dictionary<string, Dictionary<string, Dictionary<string, int?>>>> deserializer =
                ctx.GetSerializator<Dictionary<string, Dictionary<string, Dictionary<string, int?>>>>().FromJson;
            return DeserializeTime(deserializer, JsonValue);
        }
    }
}