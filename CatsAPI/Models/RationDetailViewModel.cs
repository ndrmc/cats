namespace Cats.Rest.Models
{
    public class RationDetailViewModel
    {
        public int? RationDetailID { get; set; }
        public int? RationID { get; set; }
        public int? CommodityID { get; set; }
        public string CommodityName { get; set; }
        public decimal Amount { get; set; }
        public int? UnitID { get; set; }
        public string UnitName { get; set; }
    }
}