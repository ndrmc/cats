using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class TransportRequisitionDetail
    {
        //TransportRequisitionDetailId, TransportRequisitionId, RequisitionId, RequisitionNo
        public int TransportRequisitionDetailID { get; set; }
        public int TransportRequisitionID { get; set; }
        public int RequisitionID { get; set; }

        public string RequisitionNo { get; set; }
    }
}