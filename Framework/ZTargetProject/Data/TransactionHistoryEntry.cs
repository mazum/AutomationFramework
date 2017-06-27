using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;

namespace Framework.ZTargetProject.Data
{
    public class TransactionHistoryEntry
    {
        public string Date { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public string Component { get; set; }
        public string Amount { get; set; }
        public string Balance { get; set; }
    }
}
