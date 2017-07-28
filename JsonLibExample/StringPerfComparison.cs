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

    public class StringPerfComparison : PerfComparisonBase
    {
        private const string CleanValue =  "ABC";
        private const string JsonValue =  "\"ABC\"";
        
        public override void Run(IJsonSerializators ctx)
        {
            long serNewtonsoftJson = 0L;
            string serNewtonsoftJsonExample = null;
            long serJsonLib = 0L;
            string serJsonLibExample = null;
            for (int i = 0; i < 10; i++)
            {
                var njResult = SerializeTimeNewtonsoftJson();
                serNewtonsoftJson += njResult.Item1;
                serNewtonsoftJsonExample = njResult.Item2;
                var jlResult = SerializeTimeJsonLib(ctx);
                serJsonLib += jlResult.Item1;
                serJsonLibExample = jlResult.Item2;
            }

            long deserNewtonsoftJson = 0L;
            string deserNewtonsoftJsonExample = null;
            long deserJsonLib = 0L;
            string deserJsonLibExample = null;
            for (int i = 0; i < 10; i++)
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

            Console.Out.WriteLine("################################################");
            Console.Out.WriteLine("String perf comparison");
            Console.Out.WriteLine("################################################");
            Console.Out.WriteLine("Newtonsoft.Json");
            Console.Out.WriteLine("================================================");
            Console.Out.WriteLine("  Serialize: {0}, Example: {1}", serNewtonsoftJson, serNewtonsoftJsonExample);
            Console.Out.WriteLine("Deserialize: {0}, Example: {1}", deserNewtonsoftJson, deserNewtonsoftJsonExample);
            Console.Out.WriteLine("      Total: {0}", totalNewtonsoftJson);
            Console.Out.WriteLine("################################################");
            Console.Out.WriteLine("JsonLib");
            Console.Out.WriteLine("================================================");
            Console.Out.WriteLine("  Serialize: {0}, Example: {1}", serJsonLib, serJsonLibExample);
            Console.Out.WriteLine("Deserialize: {0}, Example: {1}", deserJsonLib, deserJsonLibExample);
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
            SerializeDelegate<string> serializer = JsonConvert.SerializeObject;
            return SerializeTime(serializer, CleanValue);
        }

        private Tuple<long, string> DeserializeTimeNewtonsoftJson()
        {
            DeserializeDelegate<string> deserializer = JsonConvert.DeserializeObject<string>;
            return DeserializeTime(deserializer, JsonValue);
        }

        private Tuple<long, string> SerializeTimeJsonLib(IJsonSerializators ctx)
        {
            SerializeDelegate<string> serializer = ctx.GetJsonSerializator<string>().ToJson;
            return SerializeTime(serializer, CleanValue);
        }

        private Tuple<long, string> DeserializeTimeJsonLib(IJsonSerializators ctx)
        {
            DeserializeDelegate<string> deserializer = ctx.GetJsonSerializator<string>().FromJson;
            return DeserializeTime(deserializer, JsonValue);
        }
    }
}