namespace Cats.Rest.Models
{
    public class RegionalPsnpPlanDetailViewModel
    {
        public int RegionalPSNPPlanDetailId { get; set; }
        public int RegionalPSPNPlanID { get; set; }
        public int PlanedWoredaId { get; set; }
        public string WoredaName { get; set; }
        public int BeneficaiaryCount { get; set; }
        public int FoodRatio { get; set; }
        public int CashRatio { get; set; }
        public int Item3Ratio { get; set; }
        public int Item4Ratio { get; set; }
        public int StartingMonth { get; set; }
        public bool Contigency { get; set; }
    }
}