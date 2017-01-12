using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class TransactionGroup
    {
        public System.Guid TransactionGroupID { get; set; }
    
        public TransactionGroup(Guid transactiongroupId)
        {
            TransactionGroupID = transactiongroupId;
           
        }
    }
}