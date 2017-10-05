using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruitImportLogic
{
    public class ProcessedIncomingTransactions
    {

        public ProcessedIncomingTransactions()
        {

        }

        public ProcessedIncomingTransactions(int id, string Status, DateTime TimeProcessed, int Supplier)
        {
            this.id = id;
            this.Status = Status;
            this.TimeProcessed = TimeProcessed;
            this.Supplier = Supplier;
        }

        public int id { get; set; }

        public string Status { get; set; }

        public DateTime TimeProcessed { get; set; }

        public int Supplier { get; set; }


    }
}
