using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class TransportBidQuotation
    {
        //TransportBidQuotationId, TransportBidQuotationHeaderId, 
        //    BidId, TransporterId, SourceId, DestinationID, Tariff, IsWinner, Position, Remark
        public int TransportBidQuotationID { get; set; }
        public int TransportBidQuotationHeaderID { get; set; }
        public int BidID { get; set; }
        public int TransporterID { get; set; }
        public int SourceID { get; set; }
        public int DestinationID { get; set; }
        public decimal Tariff { get; set; }
        public bool IsWinner { get; set; }
        public int Position { get; set; }
        public string Remark { get; set; }

    }
}