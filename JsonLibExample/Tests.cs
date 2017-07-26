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
    using System.Collections.Generic;
    using com.github.zvreifnitz.JsonLib;

    public static class Tests
    {
        public static void Run(IJsonSerializators ctx)
        {
            ctx.TestString();
            ctx.TestInt();
            ctx.TestIntNullable();
            ctx.TestLong();
            ctx.TestLongNullable();
            ctx.TestGuid();
            ctx.TestGuidNullable();
            ctx.TestChar();
            ctx.TestCharNullable();
            ctx.TestListOfInt();
            ctx.TestListOfListOfString();
            ctx.TestDictionaryOfIntInt();
            ctx.TestDictionaryOfStringInt();
            ctx.TestArray();
        }

        private static void TestString(this IJsonSerializators ctx)
        {
            var serializator = ctx.GetJsonSerializator<string>();

            var input1 = "That is some input string";
            var json1 = serializator.ToJson(input1);
            var output1 = serializator.FromJson(json1);

            if (input1 != output1)
            {
                throw new Exception("String test 1 failed...");
            }

            var input2 = (string)null;
            var json2 = serializator.ToJson(input2);
            var output2 = serializator.FromJson(json2);

            if (input2 != output2)
            {
                throw new Exception("String test 2 failed...");
            }
        }

        private static void TestInt(this IJsonSerializators ctx)
        {
            var serializator = ctx.GetJsonSerializator<int>();

            var input = 123;
            var json = serializator.ToJson(input);
            var output = serializator.FromJson(json);

            if (input != output)
            {
                throw new Exception("Int test failed...");
            }
        }

        private static void TestIntNullable(this IJsonSerializators ctx)
        {
            var serializator = ctx.GetJsonSerializator<int?>();

            var input1 = (int?)123;
            var json1 = serializator.ToJson(input1);
            var output1 = serializator.FromJson(json1);

            if (input1 != output1)
            {
                throw new Exception("IntNullable test 1 failed...");
            }

            var input2 = (int?)null;
            var json2 = serializator.ToJson(input2);
            var output2 = serializator.FromJson(json2);

            if (input2 != output2)
            {
                throw new Exception("IntNullable test 2 failed...");
            }
        }

        private static void TestLong(this IJsonSerializators ctx)
        {
            var serializator = ctx.GetJsonSerializator<long>();

            var input = 123L;
            var json = serializator.ToJson(input);
            var output = serializator.FromJson(json);

            if (input != output)
            {
                throw new Exception("Long test failed...");
            }
        }

        private static void TestLongNullable(this IJsonSerializators ctx)
        {
            var serializator = ctx.GetJsonSerializator<long?>();

            var input1 = (long?)123;
            var json1 = serializator.ToJson(input1);
            var output1 = serializator.FromJson(json1);

            if (input1 != output1)
            {
                throw new Exception("LongNullable test 1 failed...");
            }

            var input2 = (long?)null;
            var json2 = serializator.ToJson(input2);
            var output2 = serializator.FromJson(json2);

            if (input2 != output2)
            {
                throw new Exception("LongNullable test 2 failed...");
            }
        }


        private static void TestGuid(this IJsonSerializators ctx)
        {
            var serializator = ctx.GetJsonSerializator<Guid>();

            var input = Guid.NewGuid();
            var json = serializator.ToJson(input);
            var output = serializator.FromJson(json);

            if (input != output)
            {
                throw new Exception("Guid test failed...");
            }
        }

        private static void TestGuidNullable(this IJsonSerializators ctx)
        {
            var serializator = ctx.GetJsonSerializator<Guid?>();

            var input1 = (Guid?)Guid.NewGuid();
            var json1 = serializator.ToJson(input1);
            var output1 = serializator.FromJson(json1);

            if (input1 != output1)
            {
                throw new Exception("GuidNullable test 1 failed...");
            }

            var input2 = (Guid?)null;
            var json2 = serializator.ToJson(input2);
            var output2 = serializator.FromJson(json2);

            if (input2 != output2)
            {
                throw new Exception("GuidNullable test 2 failed...");
            }
        }

        private static void TestChar(this IJsonSerializators ctx)
        {
            var serializator = ctx.GetJsonSerializator<char>();

            var input = 'c';
            var json = serializator.ToJson(input);
            var output = serializator.FromJson(json);

            if (input != output)
            {
                throw new Exception("Char test failed...");
            }
        }

        private static void TestCharNullable(this IJsonSerializators ctx)
        {
            var serializator = ctx.GetJsonSerializator<char?>();

            var input1 = (char?)'c';
            var json1 = serializator.ToJson(input1);
            var output1 = serializator.FromJson(json1);

            if (input1 != output1)
            {
                throw new Exception("CharNullable test 1 failed...");
            }

            var input2 = (char?)null;
            var json2 = serializator.ToJson(input2);
            var output2 = serializator.FromJson(json2);

            if (input2 != output2)
            {
                throw new Exception("CharNullable test 2 failed...");
            }
        }

        private static void TestListOfInt(this IJsonSerializators ctx)
        {
            var serializator = ctx.GetJsonSerializator<List<int>>();

            var input1 = new List<int> {246};
            var json1 = serializator.ToJson(input1);
            var output1 = serializator.FromJson(json1);

            if (input1[0] != output1[0])
            {
                throw new Exception("TestListOfInt test 1 failed...");
            }

            var input2 = new List<int>();
            var json2 = serializator.ToJson(input2);
            var output2 = serializator.FromJson(json2);

            if (input2.Count != output2.Count)
            {
                throw new Exception("TestListOfInt test 2 failed...");
            }

            var input3 = (List<int>)null;
            var json3 = serializator.ToJson(input3);
            var output3 = serializator.FromJson(json3);

            if (input3 != output3)
            {
                throw new Exception("TestListOfInt test 3 failed...");
            }
        }

        private static void TestListOfListOfString(this IJsonSerializators ctx)
        {
            var serializator = ctx.GetJsonSerializator<List<List<string>>>();

            var input1 = new List<List<string>> {new List<string> {"Here is some string!"}};
            var json1 = serializator.ToJson(input1);
            var output1 = serializator.FromJson(json1);

            if (input1[0][0] != output1[0][0])
            {
                throw new Exception("TestListOfListOfString test 1 failed...");
            }

            var input2 = (List<List<string>>)null;
            var json2 = serializator.ToJson(input2);
            var output2 = serializator.FromJson(json2);

            if (input2 != output2)
            {
                throw new Exception("TestListOfListOfString test 2 failed...");
            }
        }

        private static void TestDictionaryOfIntInt(this IJsonSerializators ctx)
        {
            var serializator = ctx.GetJsonSerializator<Dictionary<int, int>>();

            var input1 = new Dictionary<int, int> {{123, 246}, {456, 81012}};
            var json1 = serializator.ToJson(input1);
            var output1 = serializator.FromJson(json1);

            if (input1[123] != output1[123] || input1[456] != output1[456])
            {
                throw new Exception("TestDictionaryOfIntInt test 1 failed...");
            }

            var input2 = new Dictionary<int, int>();
            var json2 = serializator.ToJson(input2);
            var output2 = serializator.FromJson(json2);

            if (input2.Count != output2.Count)
            {
                throw new Exception("TestDictionaryOfIntInt test 2 failed...");
            }

            var input3 = (Dictionary<int, int>)null;
            var json3 = serializator.ToJson(input3);
            var output3 = serializator.FromJson(json3);

            if (input3 != output3)
            {
                throw new Exception("TestDictionaryOfIntInt test 3 failed...");
            }
        }

        private static void TestDictionaryOfStringInt(this IJsonSerializators ctx)
        {
            var serializator = ctx.GetJsonSerializator<Dictionary<string, int>>();

            var input1 = new Dictionary<string, int> {{"a", 246}, {"b", 81012}};
            var json1 = serializator.ToJson(input1);
            var output1 = serializator.FromJson(json1);

            if (input1["a"] != output1["a"] || input1["b"] != output1["b"])
            {
                throw new Exception("TestDictionaryOfStringInt test 1 failed...");
            }

            var input2 = new Dictionary<string, int>();
            var json2 = serializator.ToJson(input2);
            var output2 = serializator.FromJson(json2);

            if (input2.Count != output2.Count)
            {
                throw new Exception("TestDictionaryOfStringInt test 2 failed...");
            }

            var input3 = (Dictionary<string, int>)null;
            var json3 = serializator.ToJson(input3);
            var output3 = serializator.FromJson(json3);

            if (input3 != output3)
            {
                throw new Exception("TestDictionaryOfStringInt test 3 failed...");
            }
        }

        private static void TestArray(this IJsonSerializators ctx)
        {
            var serializator = ctx.GetJsonSerializator<TimeSpan?[]>();

            var input1 = new TimeSpan?[] {TimeSpan.FromMilliseconds(1), null, TimeSpan.FromMilliseconds(3)};
            var json1 = serializator.ToJson(input1);
            var output1 = serializator.FromJson(json1);

            if (input1[0] != output1[0] || input1[1] != output1[1] || input1[2] != output1[2])
            {
                throw new Exception("TestArray test 1 failed...");
            }

            var input2 = new TimeSpan?[0];
            var json2 = serializator.ToJson(input2);
            var output2 = serializator.FromJson(json2);

            if (input2.Length != output2.Length)
            {
                throw new Exception("TestArray test 2 failed...");
            }

            var input3 = (TimeSpan?[])null;
            var json3 = serializator.ToJson(input3);
            var output3 = serializator.FromJson(json3);

            if (input3 != output3)
            {
                throw new Exception("TestArray test 3 failed...");
            }
        }
    }
}