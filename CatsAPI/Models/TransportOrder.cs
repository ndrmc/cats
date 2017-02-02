using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class TransportOrder
    {
        public int TransportOrderID { get; set; }
        public string TransportOrderNo { get; set; }
        public string ContractNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequestedDispatchDate { get; set; }
        public DateTime OrderExpiryDate { get; set; }
        public string BidDocumentNo { get; set; }
        public string PerformanceBondReceiptNo { get; set; }
        public decimal? PerformanceBondAmount { get; set; }
        public int TransporterID { get; set; }
        public string ConsignerName { get; set; }
        public string TransporterSignedName { get; set; }
        public DateTime ConsignerDate { get; set; }
        public DateTime TransporterSignedDate { get; set; }
        public int? StatusID { get; set; }
        public string StatusName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? TransportRequiqsitionId { get; set; }
        public virtual ICollection<TransportOrderDetail> TransportOrderDetails { get; set; }
       
    }

}