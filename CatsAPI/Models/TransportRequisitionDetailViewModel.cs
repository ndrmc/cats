using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class TransportRequisitionDetailViewModel
    {
        public int TransportRequisitionDetailId { get; set; }
        public int TransportRequisitionId { get; set; }
        public int RequisitionId { get; set; }
        public string RequisitionNo { get; set; }
    }
}