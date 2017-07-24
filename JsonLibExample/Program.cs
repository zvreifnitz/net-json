using System;
using com.github.zvreifnitz.JsonLib;

namespace JsonLibExample
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            using (var ctx = JsonSerializationFactory.NewJsonSerializationContext())
            {
                ctx.TestString();
                ctx.TestInt();
                ctx.TestIntNullable();
                ctx.TestLong();
                ctx.TestLongNullable();
                ctx.TestGuid();
                ctx.TestGuidNullable();
            }
        }

        private static void TestString(this IJsonSerializationContext ctx)
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

        private static void TestInt(this IJsonSerializationContext ctx)
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

        private static void TestIntNullable(this IJsonSerializationContext ctx)
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

        private static void TestLong(this IJsonSerializationContext ctx)
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

        private static void TestLongNullable(this IJsonSerializationContext ctx)
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


        private static void TestGuid(this IJsonSerializationContext ctx)
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

        private static void TestGuidNullable(this IJsonSerializationContext ctx)
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
    }
}