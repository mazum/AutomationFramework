using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Framework.ZTargetProject.Data;

namespace Framework.Utils
{
    public class CsvContent<T, U>
        where U : CsvClassMap
    {
        public IList<T> Records { set; get; }
        public string[] Header { set; get; }
        public CsvContent(string filename)
        {
            ReadCsvFile(filename);
        }
        private void ReadCsvFile(string filename)
        {
            using (var csv = new CsvReader(File.OpenText(filename)))
            {
                csv.Configuration.RegisterClassMap<U>();
                csv.Configuration.WillThrowOnMissingField = false;
                Records = csv.GetRecords<T>().ToList();
                Header = csv.FieldHeaders;
            }
        }
    }
    public sealed class TransactionHistoryEntryMap : CsvClassMap<TransactionHistoryEntry>
    {
        public TransactionHistoryEntryMap()
        {
            Map(m => m.Date);
            Map(m => m.TransactionType).Name("Transaction type");
            Map(m => m.Description);
            Map(m => m.Component);
            Map(m => m.Amount);
            Map(m => m.Balance).Name("Balance*");
        }
    }
}