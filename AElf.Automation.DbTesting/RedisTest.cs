using AElf.Automation.Common.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using AElf.Automation.Common.Extensions;
using AElf.Automation.RpcTesting;
using AElf.Kernel;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using NLog;
using Hash = AElf.Automation.Common.Protobuf.Hash;

namespace AElf.Automation.DbTesting
{
    [TestClass]
    public class RedisTest
    {
        public ILogHelper Logger = LogHelper.GetLogHelper();

        [TestInitialize]
        public void InitTestLog()
        {
            string logName = "RedisTest_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".log";
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", logName);
            Logger.InitLogHelper(dir);
        }

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
            Logger.WriteInfo("Different Keys in List2");
            foreach (var item in diff2)
            {
                Logger.WriteInfo(item);
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
            Logger.WriteInfo($"list1 count: {list1.Count}");
            Logger.WriteInfo($"list2 count: {list2.Count}");
            Logger.WriteInfo($"Same count: {same.Count}");

            Logger.WriteInfo($"Diff count list1: {diff1.Count}");
            Logger.WriteInfo("Different Keys in List1");
            foreach (var item in diff1)
            {
                Logger.WriteInfo(item);
            }

            Logger.WriteInfo($"Diff count list2: {diff2.Count}");
            Logger.WriteInfo("Different Keys in List2");
            foreach (var item in diff2)
            {
                Logger.WriteInfo(item);
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
                    Logger.WriteInfo($"key: {item}");
                    Logger.WriteInfo($"value1: {hex1}");
                    Logger.WriteInfo($"value2: {hex2}");
                    Logger.WriteInfo(String.Empty);
                }
                else
                    sameCount++;
            }
            Logger.WriteInfo($"Same:{sameCount}, Diff:{diffCount}");
        }
    }
}
