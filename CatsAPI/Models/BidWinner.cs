using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class BidWinner
    {
        //BidWinnerID, BidID, SourceId, DestinationID, CommodityID, TransporterID, Amount, Tariff, Position, Status, ExpiryDate
        public int BidWinnerID { get; set; }
        public int BidID { get; set; }
        public int SourceID { get; set; }
        public int DestinationID { get; set; }
        public Nullable<int> CommodityID { get; set; }
        public int TransporterID { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> Tariff { get; set; }
        public Nullable<int> Position { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }

    }
}