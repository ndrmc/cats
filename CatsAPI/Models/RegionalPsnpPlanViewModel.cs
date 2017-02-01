using System;
using System.Collections.Generic;

namespace Cats.Rest.Models
{
    public class RegionalPsnpPlanViewModel
    {
        public int RegionalPSNPPlanID { get; set; }
        public int Year { get; set; }
        public int Duration { get; set; }
        public int RegionID { get; set; }
        public int RationId { get; set; }
        public List<RationDetailViewModel> RationDetail { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int PlanId { get; set; }
        public Guid? TransactionGroupId { get; set; }
        public List<RegionalPsnpPlanDetailViewModel> RegionalPsnpPlanDetail { get; set; }
    }
}