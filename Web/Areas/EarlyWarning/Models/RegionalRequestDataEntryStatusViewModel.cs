namespace Cats.Areas.EarlyWarning.Models
{
   public class RegionalRequestDataEntryStatusViewModel
    {
        public string Region { get; set; }
        public int TotalRequested { get; set; }
        public int Allocated { get; set; }
        public int Completed { get; set; }
        public decimal AllocationProgress { get; set; }
        public int Round { get; set; }
    }
}
