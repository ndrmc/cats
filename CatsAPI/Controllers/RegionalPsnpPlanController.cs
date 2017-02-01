using Cats.Rest.Models;
using Cats.Services.PSNP;
using System.Linq;
using System.Web.Http;

namespace Cats.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class RegionalPsnpPlanController : ApiController
    {
        private readonly IRegionalPSNPPlanService _iRegionalPSNPPlanService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iRegionalPSNPPlanService"></param>
        public RegionalPsnpPlanController(IRegionalPSNPPlanService iRegionalPSNPPlanService)
        {
            _iRegionalPSNPPlanService = iRegionalPSNPPlanService;
        }
        /// <summary>
        /// Returns list of RegionalPsnpPlans objects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetRegionalPsnpPlans()
        {
            try
            {
                var regionalPsnpPlan = _iRegionalPSNPPlanService.GetAllRegionalPSNPPlan().Select(item => new RegionalPsnpPlanViewModel
                {
                    Duration = item.Duration,
                    PlanId = item.PlanId,
                    RationDetail = item.Ration.RationDetails.Where(r => r != null).Select(detail => new RationDetailViewModel
                    {
                        Amount = detail.Amount,
                        CommodityID = detail.CommodityID,
                        CommodityName = detail.Commodity == null ? string.Empty : detail.Commodity.Name,
                        RationDetailID = detail.RationDetailID,
                        RationID = detail.RationID,
                        UnitID = detail.UnitID,
                        UnitName = detail.Unit == null ? string.Empty : detail.Unit.Name
                    }).ToList(),                   
                    RationId = item.RationID,
                    RegionalPsnpPlanDetail = item.RegionalPSNPPlanDetails.Select(detail => new RegionalPsnpPlanDetailViewModel
                    {
                        BeneficaiaryCount = detail.BeneficiaryCount,
                        CashRatio = detail.CashRatio,
                        Contigency = detail.Contingency,
                        FoodRatio = detail.FoodRatio,
                        Item3Ratio = detail.Item3Ratio,
                        Item4Ratio = detail.Item4Ratio,
                        PlanedWoredaId = detail.PlanedWoredaID,
                        RegionalPSNPPlanDetailId = detail.RegionalPSNPPlanDetailID,
                        RegionalPSPNPlanID = detail.RegionalPSNPPlanID,
                        StartingMonth = detail.StartingMonth,
                        WoredaName = detail.PlanedWoreda.Name,
                    }).ToList(),
                    RegionalPSNPPlanID = item.RegionalPSNPPlanID,
                    //RegionID = item.,
                    StatusId = item.AttachedBusinessProcess.CurrentState.StateID,
                    StatusName = item.AttachedBusinessProcess.CurrentState.BaseStateTemplate.Name,
                    TransactionGroupId = item.TransactionGroupID,
                    Year = item.Year
                }).ToList();

                return regionalPsnpPlan;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// Given an id returns a RegionalPsnpPlan object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetRegionalPsnpPlan(int id)
        {
            var obj = _iRegionalPSNPPlanService.FindById(id);
            var element = new
            {
                Duration = obj.Duration,
                PlanId = obj.PlanId,
                RationDetail = obj.Ration.RationDetails.Select(detail => new RationDetailViewModel
                {
                    Amount = detail.Amount,
                    CommodityID = detail.CommodityID,
                    CommodityName = detail.Commodity == null ? string.Empty : detail.Commodity.Name,
                    RationDetailID = detail.RationDetailID,
                    RationID = detail.RationID,
                    UnitID = detail.UnitID,
                    UnitName = detail.Unit == null ? string.Empty : detail.Unit.Name
                }).ToList(),
                RationId = obj.RationID,
                RegionalPsnpPlanDetail = obj.RegionalPSNPPlanDetails.Select(detail => new RegionalPsnpPlanDetailViewModel
                {
                    BeneficaiaryCount = detail.BeneficiaryCount,
                    CashRatio = detail.CashRatio,
                    Contigency = detail.Contingency,
                    FoodRatio = detail.FoodRatio,
                    Item3Ratio = detail.Item3Ratio,
                    Item4Ratio = detail.Item4Ratio,
                    PlanedWoredaId = detail.PlanedWoredaID,
                    RegionalPSNPPlanDetailId = detail.RegionalPSNPPlanDetailID,
                    RegionalPSPNPlanID = detail.RegionalPSNPPlanID,
                    StartingMonth = detail.StartingMonth,
                    WoredaName = detail.PlanedWoreda.Name
                }).ToList(),
                RegionalPSNPPlanID = obj.RegionalPSNPPlanID,
                //RegionID = item.RegionID,
                StatusId = obj.AttachedBusinessProcess.CurrentState.StateID,
                StatusName = obj.AttachedBusinessProcess.CurrentState.BaseStateTemplate.Name,
                TransactionGroupId = obj.TransactionGroupID,
                Year = obj.Year
            };

            return element;
        }
    }
}
