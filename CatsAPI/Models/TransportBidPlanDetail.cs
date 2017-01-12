using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class TransportBidPlanDetail
    {
        public int TransportBidPlanDetailID { get; set; }
        public int BidPlanID { get; set; }
        public int DestinationID { get; set; }
        public int SourceID { get; set; }
        public int ProgramID { get; set; }
        public decimal Quantity { get; set; }
    }
}