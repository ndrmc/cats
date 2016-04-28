using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Areas.Logistics.Models
{
    public class IncommingGRNViewModel
    {
        public string ReferenceNo { get; set; }
        public string ContractNumber { get; set; }
        public string RequisitionNo { get; set; }
        public string GRN { get; set; }
    }
}