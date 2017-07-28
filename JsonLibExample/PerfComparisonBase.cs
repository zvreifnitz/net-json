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
    using System;
    using System.Threading;
    using System.Collections.Generic;
    using com.github.zvreifnitz.JsonLib;

    public delegate string SerializeDelegate<in T>(T input);

    public delegate T DeserializeDelegate<out T>(string input);

    public abstract class PerfComparisonBase
    {
        protected const int RepetitionCount = 10;
        protected const int WarmCount = 10 * 1000;
        protected const int TestCount = 20 * 1000;
        private static readonly Random Random = new Random();
        private readonly object _tickLock = new object();

        public abstract void Run(IJsonContext ctx);

        protected Tuple<long, string> SerializeTime<T>(SerializeDelegate<T> serializer, T input)
        {
            var output = new List<string>(WarmCount + TestCount);
            for (var i = 0; i < WarmCount; i++)
            {
                var json = serializer(input);
                output.Add(json);
            }
            var start = PerformGc();
            for (var i = 0; i < TestCount; i++)
            {
                var json = serializer(input);
                output.Add(json);
            }
            var end = Ticks();
            return new Tuple<long, string>(end - start, output[Random.Next(WarmCount + TestCount)]);
        }

        protected Tuple<long, T> DeserializeTime<T>(DeserializeDelegate<T> deserializer, string input)
        {
            var output = new List<T>(WarmCount + TestCount);
            for (var i = 0; i < WarmCount; i++)
            {
                var obj = deserializer(input);
                output.Add(obj);
            }
            var start = PerformGc();
            for (var i = 0; i < TestCount; i++)
            {
                var obj = deserializer(input);
                output.Add(obj);
            }
            var end = Ticks();
            return new Tuple<long, T>(end - start, output[Random.Next(WarmCount + TestCount)]);
        }

        private long Ticks()
        {
            lock (_tickLock)
            {
                return DateTime.Now.Ticks;
            }
        }

        private long PerformGc()
        {
            GC.Collect();
            Thread.Sleep(100);
            return Ticks();
        }
    }
}