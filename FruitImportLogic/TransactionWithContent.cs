using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruitImportLogic
{
    public class TransactionWithContent
    {
        public ProcessedIncomingTransactions incomingTransaction { get; set; }
        public List<ContentOfIncomingTransaction> contentOfTransaction { get; set; }

        public TransactionWithContent(ProcessedIncomingTransactions incomingTransaction, List<ContentOfIncomingTransaction> contentOfTransaction)
        {
            this.incomingTransaction = incomingTransaction;
            this.contentOfTransaction = contentOfTransaction;
        }
    }
}
