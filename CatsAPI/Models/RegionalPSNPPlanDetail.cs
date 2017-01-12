using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class RegionalPSNPPlanDetail
    {
        //RegionalPSNPPlanDetailId,RegionalPSPNPlanID, PlanedWoredaId, WoredaName, 
        //    BeneficaiaryCount, FoodRatio, CashRatio, Item3Ratio, Item4Ratio, StartingMonth, Contigency
        public int RegionalPSNPPlanDetailID { get; set; }
        public virtual int RegionalPSNPPlanID { get; set; }
        public string WoredaName { get; set; }
        public int PlanedWoredaID { get; set; }
        public int BeneficiaryCount { get; set; }
        public int FoodRatio { get; set; }
        public int CashRatio { get; set; }
        public int Item3Ratio { get; set; }
        public int Item4Ratio { get; set; }
        public int StartingMonth { get; set; }
        public bool Contingency { get; set; }
    }
}