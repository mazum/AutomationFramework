using NetOffice.OutlookApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetOffice.OutlookApi.Enums;
using Framework.Extensions;
using Exception = System.Exception;

namespace Framework.Utils
{
    public class MailAdapter
    {
        public static MailItem GetEmail(string subject, string recipientEmail, DateTime maxAgeTimeStamp, int waitSeconds)
        {
            const int checkIntervalSeconds = 15;
            const string prSmtpAddress = "http://schemas.microsoft.com/mapi/proptag/0x39FE001E";

            if (Process.GetProcessesByName("outlook").Length == 0)
            {
                Process.Start("outlook.exe");
            }

            var item = new MailItem();
            NetOffice.OutlookApi.MailItem oItem = null;
            var match = false;
            try
            {
                Application app = new Application();
                _NameSpace ns = app.GetNamespace("MAPI");
                ns.Logon(null, null, false, false);

                MAPIFolder inboxFolder = ns.GetDefaultFolder(OlDefaultFolders.olFolderInbox);
                _Items oItems = inboxFolder.Items;
                oItems.Sort("[ReceivedTime]", false);
                Console.WriteLine("DBG: Started looking for email at {0}", DateTime.Now);
                for (int j = 0; j <= waitSeconds; j = j + checkIntervalSeconds)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(checkIntervalSeconds));
                    for (int i = oItems.Count; i > 1; i--)
                    {
                        oItem = (NetOffice.OutlookApi.MailItem)oItems[i];
                        Console.WriteLine("DBG: Checking mail at {0} => found mail with timestamp: { 1}", DateTime.Now.ToLongTimeString(), oItem.SentOn.ToLongTimeString());
                        if ((oItem.ReceivedTime - maxAgeTimeStamp).TotalSeconds < 0) break;
                        if (oItem.Subject.IsRegExMatch(subject) && oItem.Recipients.Single().PropertyAccessor.GetProperty(prSmtpAddress).ToString().Equals(recipientEmail))
                        {
                            match = true;
                            break;
                        }
                        //Console.WriteLine("Subject: {0}", item.Subject);
                        //Console.WriteLine("Sent: {0} {1}", item.SentOn.ToLongDateString(), item.SentOn.ToLongTimeString());
                    }
                    if (match) break;
                }
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                throw new Exception("ERROR: retrieving the email failed due to System.Runtime.InteropServices.COMException");
            }

            if (match)
            {
                item.EntryId = oItem.EntryID;
                item.Subject = oItem.Subject;
                item.SentDate = oItem.SentOn.ToLongDateString();
                item.SentTime = oItem.SentOn.ToLongTimeString();
                item.ReceivedDate = oItem.ReceivedTime.ToLongDateString();
                item.ReceivedTime = oItem.ReceivedTime.ToLongTimeString();
                item.Body = oItem.Body;
                item.HtmlBody = oItem.HTMLBody;
                item.HasMessage = true;
                item.PrintInfo();
            }
            else
            {
                Console.WriteLine("DBG: Couldn't find message with subject matching {0} and recipient {1}", subject, recipientEmail);
            }
            Console.WriteLine("DBG: Finished looking for email at {0}", DateTime.Now);
            return item;
        }
    }

    public class MailItem
    {
        public MailItem()
        {
            HasMessage = false;
        }
        public string EntryId { get; set; }
        public string Subject { get; set; }
        public string SentDate { get; set; }
        public string SentTime { get; set; }
        public string ReceivedDate { get; set; }
        public string ReceivedTime { get; set; }
        public string Body { get; set; }
        public string HtmlBody { get; set; }
        public bool HasMessage { get; set; }
        public void PrintInfo()
        {
            Console.WriteLine("Subject:\t{0}", Subject);
            Console.WriteLine("Sent:\t\t{0} {1}", SentDate, SentTime);
            Console.WriteLine("Received:\t{0} {1}", ReceivedDate, ReceivedTime);
            Console.WriteLine("Body:\n{0}", Body);
        }
    }
}