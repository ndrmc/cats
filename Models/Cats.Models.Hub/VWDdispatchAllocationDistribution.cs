using System;

namespace Cats.Models.Hubs
{
    public class VWDdispatchAllocationDistribution
    {
       public string Transporter { get; set; }
        public string Commodity { get; set; }
        public DateTime DispatchDate { get; set; }
        public decimal AllocatedAmount { get; set; }
        public decimal DispatchedAmount { get; set; }
        public decimal Diff1 { get; set; } // the difference between allocated amount and dispatched amount
        //public decimal Diff2 { get; set; } // the discripancy that is between to be dispatched amount and actually dispatched, not yet implemented
        //public string Fdp { get; set; }
        //public string Hub { get; set; }
    }
}
