using AElf.Automation.Common.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AElf.Automation.DbTesting
{
    [TestClass]
    public class RedisTest
    {
        [TestMethod]
        public void CompareTheSame1()
        {
            var rh1 = new RedisHelper("192.168.197.34");
            var rh2 = new RedisHelper("192.168.197.29");
            var list1 = rh1.GetAllKeys();
            var list2 = rh2.GetAllKeys();

            var same = RedisHelper.GetIntersection(list1, list2);
            var diff1 = RedisHelper.GetExceptList(list1, list2);
            var diff2 = RedisHelper.GetExceptList(list2, list1);
            Console.WriteLine($"list1 count: {list1.Count}");
            Console.WriteLine($"list2 count: {list2.Count}");
            Console.WriteLine($"Same count: {same.Count}");

            Console.WriteLine($"Diff count list1: {diff1.Count}");
            Console.WriteLine("Different Keys in List1");
            foreach (var item in diff1)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($"Diff count list2: {diff2.Count}");
            Console.WriteLine("Different Keys in List2");
            foreach (var item in diff2)
            {
                Console.WriteLine(item);
            }

            Assert.IsTrue(diff1.Count <2, "192.168.197.34 diff");
            Assert.IsTrue(diff2.Count <2, "192.168.197.20 diff");
        }

        [TestMethod]
        public void CompareTheSame2()
        {
            var rh1 = new RedisHelper("192.168.197.34");
            var rh2 = new RedisHelper("192.168.197.13");
            var list1 = rh1.GetAllKeys();
            var list2 = rh2.GetAllKeys();

            var same = RedisHelper.GetIntersection(list1, list2);
            var diff1 = RedisHelper.GetExceptList(list1, list2);
            var diff2 = RedisHelper.GetExceptList(list2, list1);
            Console.WriteLine($"list1 count: {list1.Count}");
            Console.WriteLine($"list2 count: {list2.Count}");
            Console.WriteLine($"Same count: {same.Count}");

            Console.WriteLine($"Diff count list1: {diff1.Count}");
            Console.WriteLine("Different Keys in List1");
            foreach (var item in diff1)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($"Diff count list2: {diff2.Count}");
            Console.WriteLine("Different Keys in List2");
            foreach (var item in diff2)
            {
                Console.WriteLine(item);
            }

            Assert.IsTrue(diff1.Count < 2, "192.168.197.34 diff");
            Assert.IsTrue(diff2.Count < 2, "192.168.197.13 diff");
        }

        [TestMethod]
        public void CompareTheSame3()
        {
            var rh1 = new RedisHelper("192.168.197.29");
            var rh2 = new RedisHelper("192.168.197.13");
            var list1 = rh1.GetAllKeys();
            var list2 = rh2.GetAllKeys();

            var same = RedisHelper.GetIntersection(list1, list2);
            var diff1 = RedisHelper.GetExceptList(list1, list2);
            var diff2 = RedisHelper.GetExceptList(list2, list1);
            Console.WriteLine($"list1 count: {list1.Count}");
            Console.WriteLine($"list2 count: {list2.Count}");
            Console.WriteLine($"Same count: {same.Count}");

            Console.WriteLine($"Diff count list1: {diff1.Count}");
            Console.WriteLine("Different Keys in List1");
            foreach (var item in diff1)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($"Diff count list2: {diff2.Count}");
            Console.WriteLine("Different Keys in List2");
            foreach (var item in diff2)
            {
                Console.WriteLine(item);
            }

            Assert.IsTrue(diff1.Count < 2, "192.168.197.20 diff");
            Assert.IsTrue(diff2.Count < 2, "192.168.197.13 diff");
        }

        [TestMethod]
        public void CompareDbValue()
        {
            var rh1 = new RedisHelper("192.168.197.34");
            var rh2 = new RedisHelper("192.168.197.29");
            var list1 = rh1.GetAllKeys();
            var list2 = rh2.GetAllKeys();

            var same = RedisHelper.GetIntersection(list1, list2);
            int sameCount = 0;
            int diffCount = 0;
            foreach(var item in same)
            {
                var byteInfo1 = rh1.GetT<byte[]>(item);
                var byteInfo2 = rh2.GetT<byte[]>(item);
                string hex1 = BitConverter.ToString(byteInfo1, 0).Replace("-", string.Empty).ToLower();
                string hex2 = BitConverter.ToString(byteInfo2, 0).Replace("-", string.Empty).ToLower();
                if (hex1 != hex2)
                {
                    diffCount++;
                    Console.WriteLine($"key: {item}");
                    Console.WriteLine($"value1: {hex1}");
                    Console.WriteLine($"value2: {hex2}");
                    Console.WriteLine();
                }
                else
                    sameCount++;

            }
            Console.WriteLine($"Same:{sameCount}, Diff:{diffCount}");

        }
    }
}
