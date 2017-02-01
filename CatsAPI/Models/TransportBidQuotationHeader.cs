using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class TransportBidQuotationHeader
    {
        public int TransportBidQuotationHeaderID { get; set; }
        public Nullable<System.DateTime> BidQuotationDate { get; set; }
        public decimal BidBondAmount { get; set; }
        public Nullable<int> TransporterId { get; set; }
        public string TransporterName { get; set; }
        public Nullable<int> BidId { get; set; }
        public Nullable<int> RegionID { get; set; }
        public string RegionName { get; set; }
        public string EnteredBy { get; set; }
        public int Status { get; set; }
        public string statusName { get; set; }
        public virtual ICollection<TransportBidQuotation> TransportBidQuotations { get; set; }
    }
}