using System;
using System.Collections.Generic;

namespace Cats.Rest.Models
{
    public class BidModelView
    {
        public int BidID { get; set; }
        public int RegionID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BidNumber { get; set; }
        public DateTime OpeningDate { get; set; }
        public int StatusID { get; set; }
        public string Status { get; set; }
        public int TransportBidPlanID { get; set; }
        public decimal BidBondAmount { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string BidOpeningTime { get; set; }
        public List<BidDetailModelView> BidDetails { get; set; }
    }
}