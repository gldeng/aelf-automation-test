﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AElf.Automation.Common.Extensions
{
    public class CommandInfo
    {
        public string Category { get; set; }
        public string Cmd { get; set; }
        public string Parameter { get; set; }

        public bool Result { get; set; }
        public JObject JsonInfo { get; set; }
        public List<string> InfoMsg { get; set; }
        public List<string> ErrorMsg { get; set; }
        public long TimeSpan { get; set; }

        public CommandInfo(string cmd, string category="")
        {
            Category = (category == "") ? cmd : category; 
            Cmd = cmd;
            Parameter = string.Empty;
            InfoMsg = new List<string>();
            ErrorMsg = new List<string>();
            Result = false;
            TimeSpan = 0;
        }
        
        public void GetJsonInfo()
        {
            if(Result)
                JsonInfo = JsonConvert.DeserializeObject<JObject>(InfoMsg[0]);
            else
                JsonInfo = JsonConvert.DeserializeObject<JObject>(ErrorMsg[0]);                
        }

        public void PrintResultMessage()
        {
            if (Result)
            {
                Console.WriteLine("{0} : {1}", Category, "Pass");
                foreach(var item in InfoMsg)
                    Console.WriteLine(item);
            }
            else
            {
                Console.WriteLine("{0} : {1}", Category, "Failed");
                foreach(var item in ErrorMsg)
                    Console.WriteLine(item);
            }
        }
        
        public bool CheckParameterValid(int count)
        {
            if (count == 1 && Parameter.Trim() == "")
                return false;
            
            var paraArray = Parameter.Split(" ");
            if (paraArray.Length != count)
            {
                ErrorMsg.Add("Parameter error.");
                Console.WriteLine("{0} command parameter is invalid.", Category);
                return false;
            }
            return true;
        }
    }
    
    public class CategoryRequest
    {
        public string Category { get; set; }
        public int Count { get; set; }
        public int PassCount { get; set; }
        public int FailCount { get; set; }
        public long TotalTimeInfo { get; set; }
        public long AvageTimeInfo { get; set; }

        public List<CommandInfo> Commands { get; set; }

        public CategoryRequest()
        {
            Commands = new List<CommandInfo>();
            Count = 0;
            PassCount = 0;
            FailCount = 0;
            TotalTimeInfo = 0;
            AvageTimeInfo = 0;
        }
     }

    public class CategoryInfoSet
    {
        public List<CommandInfo> CommandList { get; set; }
        public List<CategoryRequest> CategoryList { get; set; }

        public CategoryInfoSet(List<CommandInfo> commands)
        {
            CommandList = commands;
            CategoryList = new List<CategoryRequest>();
        }

        public void GetCategoryBasicInfo()
        {
            foreach (var item in CommandList)
            {
                var category = CategoryList.FindLast(x => x.Category == item.Category);
                if (category == null)
                {
                    category = new CategoryRequest();
                    category.Category = item.Category;
                    CategoryList.Add(category);
                }

                category.Commands.Add(item);
            }
        }

        public void GetCategorySummaryInfo()
        {
            foreach (var item in CategoryList)
            {
                Console.WriteLine("Rpc Category: {0}", item.Category);
                item.Count = item.Commands.Count;
                item.PassCount = item.Commands.FindAll(x => x.Result == true).Count;
                item.FailCount = item.Commands.FindAll(x => x.Result == false).Count;
                foreach (var command in item.Commands.FindAll(x => x.Result == true))
                {
                    item.TotalTimeInfo += command.TimeSpan;
                }

                if (item.PassCount != 0)
                    item.AvageTimeInfo = item.TotalTimeInfo / item.PassCount;
                else
                    item.AvageTimeInfo = 0;

                Console.WriteLine("Total count: {0}", item.Count);
                Console.WriteLine("Pass count: {0}", item.PassCount);
                Console.WriteLine("Fail count: {0}", item.FailCount);
                Console.WriteLine("AvageTime(milesecond): {0}", item.AvageTimeInfo);
            }
        }
        
        public void SaveTestResultXml(int threadCount)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            var el = xmlDoc.CreateElement("RpcApiResults");
            xmlDoc.AppendChild(el);

            XmlAttribute thread = xmlDoc.CreateAttribute("ThreadCount");
            thread.Value = threadCount.ToString();
            el.Attributes.Append(thread);

            foreach (var item in CategoryList)
            {
                XmlElement rpc = xmlDoc.CreateElement("RpcApi");

                XmlAttribute category = xmlDoc.CreateAttribute("Category");
                category.Value = item.Category;

                XmlAttribute totalTimes = xmlDoc.CreateAttribute("TotalTimes");
                totalTimes.Value = item.Count.ToString();

                XmlAttribute avageTime = xmlDoc.CreateAttribute("AvageTime");
                avageTime.Value = item.AvageTimeInfo.ToString();

                rpc.Attributes.Append(category);
                rpc.Attributes.Append(totalTimes);
                rpc.Attributes.Append(avageTime);

                XmlElement totalTimeInfo = xmlDoc.CreateElement("TotalExeTime");
                totalTimeInfo.InnerText = item.TotalTimeInfo.ToString();

                XmlElement passCount = xmlDoc.CreateElement("PassCount");
                passCount.InnerText = item.PassCount.ToString();

                XmlElement failCount = xmlDoc.CreateElement("FailCount");
                failCount.InnerText = item.FailCount.ToString();

                rpc.AppendChild(totalTimeInfo);
                rpc.AppendChild(passCount);
                rpc.AppendChild(failCount);

                el.AppendChild(rpc);
            }
            string fileName = "RpcResult_Thread_" + threadCount+"_" + DateTime.Now.Millisecond.ToString() + ".xml";
            xmlDoc.Save(fileName);
        }
    }
}
