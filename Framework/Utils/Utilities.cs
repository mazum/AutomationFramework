using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using Framework.Dao;

namespace Framework.Utils
{
    public static class Utilities
    {
        public static void KillWebDriverZombieProcesses()
        {
            var processes = new List<Process>();
            if (AppSettings.Current.Browser.Equals("phantom"))
                processes.AddRange(Process.GetProcessesByName("phantomjs")); // used for healthcheck which might run on the same test server
            else
            {
                processes.AddRange(Process.GetProcessesByName("chromedriver"));
                processes.AddRange(Process.GetProcessesByName("IEDriverServer"));
                // processes.AddRange(Process.GetProcessesByName("iexplore"));
            }
            var currentProcessId = Process.GetCurrentProcess().Id;
            foreach (var process in processes)
                if (process.Id != currentProcessId)
                    try
                    {
                        process.Kill();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: Couldn't kill process. " + e);
                    }
        }

        public static FileInfo RetrieveDownloadedFileAttributes(DirectoryInfo directory, string fileNameSubstring)
        {
            FileInfo result = null;
            var list = directory.GetFiles("*" + fileNameSubstring + "*");
            for (int i = 0; i < 10; i++)
                if (list.Any())
                {
                    result = list.OrderByDescending(f => f.LastWriteTime).First();
                    if (result.Extension.Equals(".crdownload"))
                    {
                        Thread.Sleep(1000);
                        list = directory.GetFiles("*" + fileNameSubstring + "*");
                    }
                    else
                    {
                        break;
                    }
                }
            if (result == null)
                throw new FileNotFoundException("File not found with '" + fileNameSubstring + "' in filename");
            return result;
        }

        public static void PrintCookies(IWebDriver driver)
        {
            if (AppSettings.Current.EnableCookieInfoLogging)
            {
                Console.WriteLine("Cookies -------------");
                foreach (var c in driver.Manage().Cookies.AllCookies)
                    Console.WriteLine("Cookie: {0}, {1}, {2}", c.Name, c.Domain, c.Value);
                Console.WriteLine("Ende -------------");
            }
        }

        public static DateTime FinancialYearStartDate()
        {
            const int fyStartMonth = 7;
            DateTime dte = new DateTime(DateTime.Today.Year, fyStartMonth, 1); // 1st July this year

            if ((DateTime.Today.Month < fyStartMonth))
                dte = dte.AddYears(-1);

            return dte;
        }

        public static string GetMessageFromMessageSpecFlowTable(Table table, bool hasMultiAccount)
        {
            return table.Rows.First(r => Convert.ToBoolean(r["Multi Account"]).Equals(hasMultiAccount))["Message"];
        }

        public static Tuple<string, DateTime> GetGUIDDetails(string MemberNumber, string ProcessAction = "Onboarding")
        {
            var guidDetails = new Tuple<string, DateTime>(
                GetValueFromSqlQuery("select UniqueURLGUID from ODS_Channels.Super.UniqueURLs where MemberNumber =\'" +
                                     MemberNumber + "\' and ProcessAction=\'" + ProcessAction +
                                     "\' order by LastModifiedDateTime desc"),
                DateTime.ParseExact(
                    GetValueFromSqlQuery(
                        "select ExpiryDateTime from ODS_Channels.Super.UniqueURLs where MemberNumber =\'" +
                        MemberNumber + "\' and ProcessAction=\'" + ProcessAction +
                        "\' order by LastModifiedDateTime desc"), "M/d/yyyyhh: mm:ss tt", null));
            return guidDetails;
        }

        public static string GetValueFromSqlQuery(string sqlQuery)
        {
            var baseDao = new BaseDao { ConnectionString = ConfigurationManager.AppSettings["MPSConnectionString"] };
            return baseDao.GetValueFromSqlQuery(sqlQuery);
        }
    }
}