using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cats.Models;

namespace Cats.Areas.Procurement.Models
{
    public class BidsViewModel
    {
        public int BidID { get; set; }
        public string BidNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string OpeningDate { get; set; }
        public int StatusID { get; set; }
        public List<BidDetail> BidDetails { get; set; }
        public string Time { get; set; }
    }

    public class StatusViewmodel
    {
        // BidStatus
        public int StatusId { get; set; }
        public string Status { get; set; }

    }
    
    public class BidWinnerRank
    {
      public  int Id { get; set; }
      public  string Name { get; set; }
    }
    public class DataEntryViewModel
    {
        public int BidID { get; set; }
        public string BidNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string OpeningDate { get; set; }
        public string TransporterName { get; set; }
        public List<TransportBidQuotationHeader> TransportBidQuotationHeaders { get; set; }
        public int NoOfWoredaWithRFQ { get; set; }
        public int  NoOfExpectedWoreda { get; set; }
        public int StatusID { get; set; }
        public List<BidDetail> BidDetails { get; set; }
        public string Time { get; set; }
    }
}