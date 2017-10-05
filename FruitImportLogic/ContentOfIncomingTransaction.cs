using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruitImportLogic
{
    public class ContentOfIncomingTransaction
    {
        public int id { get; set; }

        public int Fruit { get; set; }

        public int ProcessedIncomingTransactions { get; set; }

        public int Amount { get; set; }

        public ContentOfIncomingTransaction(int fruit, int amount)
        {
            this.Fruit = fruit;
            this.Amount = amount;
        }

    }
}
