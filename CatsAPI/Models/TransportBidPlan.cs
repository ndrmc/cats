using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class TransportBidPlan
    {
        public int TransportBidPlanID { get; set; }
        public int Year { get; set; }
        public int YearHalf { get; set; }
        public int ProgramID { get; set; }
        public string ProgramName { get; set; }
        public virtual List<TransportBidPlanDetail> TransportBidPlanDetails { get; set; }
    }
}