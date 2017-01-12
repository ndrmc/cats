using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class Transaction
    {
        public Guid TransactionID { get; set; }
        public Guid? TransactionGroupID { get; set; }

        public Transaction(Guid transactionId, Guid? transactionGroupId)
        {
            TransactionGroupID = transactionGroupId;
            TransactionID = transactionId;
        }
    }
}