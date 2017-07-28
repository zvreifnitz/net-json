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
    using Newtonsoft.Json;
    using System;
    using com.github.zvreifnitz.JsonLib;

    public class SimpleObjectPerfComparison : PerfComparisonBase
    {
        private static readonly SimpleObject CleanValue =
            new SimpleObject
            {
                IntValue = 1,
                GuidValue = Guid.Parse("a50229d4-220d-481e-969e-772f3797ae85"),
                StringValue = "ABC"
            };

        private const string JsonValue = "{\"IntValue\":1,\"GuidValue\":\"a50229d4-220d-481e-969e-772f3797ae85\",\"StringValue\":\"ABC\"}";

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
            SimpleObject deserNewtonsoftJsonExample = null;
            long deserJsonLib = 0L;
            SimpleObject deserJsonLibExample = null;
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
            Console.Out.WriteLine("SimpleObject perf comparison");
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
            SerializeDelegate<SimpleObject> serializer = JsonConvert.SerializeObject;
            return SerializeTime(serializer, CleanValue);
        }

        private Tuple<long, SimpleObject> DeserializeTimeNewtonsoftJson()
        {
            DeserializeDelegate<SimpleObject> deserializer = JsonConvert.DeserializeObject<SimpleObject>;
            return DeserializeTime(deserializer, JsonValue);
        }

        private Tuple<long, string> SerializeTimeJsonLib(IJsonContext ctx)
        {
            SerializeDelegate<SimpleObject> serializer = ctx.GetSerializator<SimpleObject>().ToJson;
            return SerializeTime(serializer, CleanValue);
        }

        private Tuple<long, SimpleObject> DeserializeTimeJsonLib(IJsonContext ctx)
        {
            DeserializeDelegate<SimpleObject> deserializer = ctx.GetSerializator<SimpleObject>().FromJson;
            return DeserializeTime(deserializer, JsonValue);
        }
    }
}