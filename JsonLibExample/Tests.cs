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
    using System;
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
    }
}