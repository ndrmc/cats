using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class Ledger
    {
        //LedgerId, Name, LedgerTypeID, LedgerTypeName
        public int LedgerID { get; set; }
        public string Name { get; set; }
        public int LedgerTypeID { get; set; }
        public string LedgerTypeName { get; set; }
    }
}