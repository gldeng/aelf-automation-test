using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AElf.Automation.Common.Extensions;
using AElf.Automation.Common.Helpers;
using AElf.Automation.RpcTesting;

namespace AElf.Automation.RedisTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            var rt = new RedisTest();
            rt.InitTestLog();
            rt.ScanDBInformation("192.168.199.221", "http://192.168.199.221:8000/chain");
        }
    }

    public class RedisTest
    {
        public ILogHelper Logger = LogHelper.GetLogHelper();

        public void InitTestLog()
        {
            string logName = "RedisTest_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".log";
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", logName);
            Logger.InitLogHelper(dir);
        }

        public void QueryRedisKeyInfo(string redishost, string key)
        {
        }

        public void ScanDBInformation(string redishost, string rpcUrl)
        {

        }
    }
}