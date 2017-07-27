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
    using System.Threading;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using com.github.zvreifnitz.JsonLib;

    public static class StringPerfComparison
    {
        private static readonly object TickLock = new object();

        public static void Run(IJsonSerializators ctx)
        {
            long serNewtonsoftJson = 0L;
            long serJsonLib = 0L;
            for (int i = 0; i < 10; i++)
            {
                serNewtonsoftJson += SerializeTimeNewtonsoftJson();
                serJsonLib += SerializeTimeJsonLib(ctx);
            }

            long deserNewtonsoftJson = 0L;
            long deserJsonLib = 0L;
            for (int i = 0; i < 10; i++)
            {
                deserNewtonsoftJson += DeserializeTimeNewtonsoftJson();
                deserJsonLib += DeserializeTimeJsonLib(ctx);
            }

            long totalNewtonsoftJson = serNewtonsoftJson + deserNewtonsoftJson;
            long totalJsonLib = serJsonLib + deserJsonLib;

            Console.Out.WriteLine("================================================");
            Console.Out.WriteLine("Newtonsoft.Json");
            Console.Out.WriteLine("================================================");
            Console.Out.WriteLine("Serialize: {0}", serNewtonsoftJson);
            Console.Out.WriteLine("Deserialize: {0}", deserNewtonsoftJson);
            Console.Out.WriteLine("Total: {0}", totalNewtonsoftJson);
            Console.Out.WriteLine("================================================");
            Console.Out.WriteLine("JsonLib");
            Console.Out.WriteLine("================================================");
            Console.Out.WriteLine("Serialize: {0}", serJsonLib);
            Console.Out.WriteLine("Deserialize: {0}", deserJsonLib);
            Console.Out.WriteLine("Total: {0}", totalJsonLib);
            Console.Out.WriteLine("================================================");
            Console.Out.WriteLine("Ratio");
            Console.Out.WriteLine("================================================");
            Console.Out.WriteLine("Serialize: {0}", (double)serNewtonsoftJson / serJsonLib);
            Console.Out.WriteLine("Deserialize: {0}", (double)deserNewtonsoftJson / deserJsonLib);
            Console.Out.WriteLine("Total: {0}", (double)totalNewtonsoftJson / totalJsonLib);
        }

        private static long SerializeTimeNewtonsoftJson()
        {
            int warmCount = 10 * 1000;
            int testCount = 100 * 1000;
            Type stringType = typeof(string);
            string input = "ABC";
            List<String> output = new List<string>(warmCount + testCount);
            for (int i = 0; i < warmCount; i++)
            {
                string json = JsonConvert.SerializeObject(input, stringType, null);
                output.Add(json);
            }
            long start = GC();
            for (int i = 0; i < testCount; i++)
            {
                string json = JsonConvert.SerializeObject(input, stringType, null);
                output.Add(json);
            }
            long end = Ticks();
            Console.Out.WriteLine("================================================");
            Console.Out.WriteLine("Newtonsoft.Json");
            Console.Out.WriteLine("================================================");
            Console.Out.WriteLine(output[new Random().Next(warmCount + testCount)]);
            var time = end - start;
            Console.Out.WriteLine("Serialize Time: {0}", time);
            return time;
        }

        private static long DeserializeTimeNewtonsoftJson()
        {
            int warmCount = 10 * 1000;
            int testCount = 100 * 1000;
            string input = "\"ABC\"";
            List<String> output = new List<string>(warmCount + testCount);
            for (int i = 0; i < warmCount; i++)
            {
                string obj = JsonConvert.DeserializeObject<string>(input);
                output.Add(obj);
            }
            long start = GC();
            for (int i = 0; i < testCount; i++)
            {
                string obj = JsonConvert.DeserializeObject<string>(input);
                output.Add(obj);
            }
            long end = Ticks();
            Console.Out.WriteLine("================================================");
            Console.Out.WriteLine("Newtonsoft.Json");
            Console.Out.WriteLine("================================================");
            Console.Out.WriteLine(output[new Random().Next(warmCount + testCount)]);
            var time = end - start;
            Console.Out.WriteLine("Deserialize Time: {0}", time);
            return time;
        }

        private static long SerializeTimeJsonLib(IJsonSerializators ctx)
        {
            int warmCount = 10 * 1000;
            int testCount = 100 * 1000;
            string input = "ABC";
            List<String> output = new List<string>(warmCount + testCount);
            for (int i = 0; i < warmCount; i++)
            {
                string json = ctx.GetJsonSerializator<string>().ToJson(input);
                output.Add(json);
            }
            long start = GC();
            for (int i = 0; i < testCount; i++)
            {
                string json = ctx.GetJsonSerializator<string>().ToJson(input);
                output.Add(json);
            }
            long end = Ticks();
            Console.Out.WriteLine("================================================");
            Console.Out.WriteLine("JsonLib");
            Console.Out.WriteLine("================================================");
            Console.Out.WriteLine(output[new Random().Next(warmCount + testCount)]);
            var time = end - start;
            Console.Out.WriteLine("Serialize Time: {0}", time);
            return time;
        }

        private static long DeserializeTimeJsonLib(IJsonSerializators ctx)
        {
            int warmCount = 10 * 1000;
            int testCount = 100 * 1000;
            string input = "\"ABC\"";
            List<String> output = new List<string>(warmCount + testCount);
            for (int i = 0; i < warmCount; i++)
            {
                string obj = ctx.GetJsonSerializator<string>().FromJson(input);
                output.Add(obj);
            }
            long start = GC();
            for (int i = 0; i < testCount; i++)
            {
                string obj = ctx.GetJsonSerializator<string>().FromJson(input);
                output.Add(obj);
            }
            long end = Ticks();
            Console.Out.WriteLine("================================================");
            Console.Out.WriteLine("JsonLib");
            Console.Out.WriteLine("================================================");
            Console.Out.WriteLine(output[new Random().Next(warmCount + testCount)]);
            var time = end - start;
            Console.Out.WriteLine("Deserialize Time: {0}", time);
            return time;
        }

        private static long Ticks()
        {
            lock (TickLock)
            {
                return DateTime.Now.Ticks;
            }
        }

        private static long GC()
        {
            System.GC.Collect();
            Thread.Sleep(100);
            return Ticks();
        }
    }
}