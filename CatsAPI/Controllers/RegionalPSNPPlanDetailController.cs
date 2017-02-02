using Cats.Rest.Models;
using Cats.Services.PSNP;
using System.Linq;
using System.Web.Http;

namespace Cats.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class RegionalPsnpPlanDetailController : ApiController
    {
        private readonly IRegionalPSNPPlanDetailService _iRegionalPSNPPlanDetailService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iRegionalPSNPPlanDetailService"></param>
        public RegionalPsnpPlanDetailController(IRegionalPSNPPlanDetailService iRegionalPSNPPlanDetailService)
        {
            _iRegionalPSNPPlanDetailService = iRegionalPSNPPlanDetailService;
        }
        /// <summary>
        /// Returns list of RegionalPsnpPlanDetails objects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetRegionalPsnpPlanDetails()
        {
            try
            {
                var regionalPsnpPlan = _iRegionalPSNPPlanDetailService.GetAllRegionalPSNPPlanDetail().Select(item => new RegionalPsnpPlanDetailViewModel
                {
                    BeneficaiaryCount = item.BeneficiaryCount,
                    CashRatio = item.CashRatio,
                    Contigency = item.Contingency,
                    FoodRatio = item.FoodRatio,
                    Item3Ratio = item.Item3Ratio,
                    Item4Ratio = item.Item4Ratio,
                    PlanedWoredaId = item.PlanedWoredaID,
                    RegionalPSNPPlanDetailId = item.RegionalPSNPPlanDetailID,
                    RegionalPSPNPlanID = item.RegionalPSNPPlanID,
                    StartingMonth = item.StartingMonth,
                    WoredaName = item.PlanedWoreda.Name
                });

                return regionalPsnpPlan;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Given an id returns a RegionalPsnpPlanDetail object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetRegionalPsnpPlanDetail(int id)
        {
            var obj = _iRegionalPSNPPlanDetailService.FindById(id);
            var element = new
            {
                BeneficaiaryCount = obj.BeneficiaryCount,
                CashRatio = obj.CashRatio,
                Contigency = obj.Contingency,
                FoodRatio = obj.FoodRatio,
                Item3Ratio = obj.Item3Ratio,
                Item4Ratio = obj.Item4Ratio,
                PlanedWoredaId = obj.PlanedWoredaID,
                RegionalPSNPPlanDetailId = obj.RegionalPSNPPlanDetailID,
                RegionalPSPNPlanID = obj.RegionalPSNPPlanID,
                StartingMonth = obj.StartingMonth,
                WoredaName = obj.PlanedWoreda.Name
            };

            return element;
        }
    }
}
