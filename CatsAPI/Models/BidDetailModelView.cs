using System;


namespace Cats.Rest.Models
{
    public class BidDetailModelView
    {
        public int BidDetailId { get; set; }
        public decimal AmountForReliefProgram { get; set; }
        public decimal AmountForPSNPProgram { get; set; }
        public decimal BidDocumentPrice { get; set; }
        public decimal CPO { get; set; }
    }
}