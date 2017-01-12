using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class LedgerType
    {
        public int LedgerTypeID { get; set; }
        public string Name { get; set; }
        public string Direction { get; set; }
    }
}