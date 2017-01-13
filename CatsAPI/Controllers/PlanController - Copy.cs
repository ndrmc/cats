using Cats.Services.PSNP;
using System.Linq;
using System.Web.Http;

namespace Cats.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class RegionalPSNPPlanServiceontroller : ApiController
    {
        private readonly IRegionalPSNPPlanService _iregionalPsnpPlanService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iregionalPsnpPlanService"></param>
        public RegionalPSNPPlanServiceontroller(IRegionalPSNPPlanService iregionalPsnpPlanService)
        {
            _iregionalPsnpPlanService = iregionalPsnpPlanService;
        }
        /// <summary>
        /// Returns list of RegionalPsnpPlan objects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetRegionalPSNPPlans()
        {
            var bids = _iregionalPsnpPlanService.GetAllRegionalPSNPPlan().Select(item => new
            {
                item.RegionalPSNPPlanID,
                item.Year,
                item.Duration,
                item.,
                item.RationID,
                item.Ration.RationDetails,
                item.AttachedBusinessProcess.CurrentStateID,
                item.AttachedBusinessProcess.CurrentState.BaseStateTemplate.Name,
                item.PlanId,
                item.TransactionGroupID,
                item.RegionalPSNPPlanDetails
            }).ToList();

            return bids;
        }
        /// <summary>
        /// Given an id returns a RegionalPsnpPlan object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetRegionalPSNPPlan(int id)
        {
            var item = _iregionalPsnpPlanService.FindById(id);
            var element = new
            {
                item.RegionalPSNPPlanID,
                item.Year,
                item.Duration,
                item.,
                item.RationID,
                item.Ration.RationDetails,
                item.AttachedBusinessProcess.CurrentStateID,
                item.AttachedBusinessProcess.CurrentState.BaseStateTemplate.Name,
                item.PlanId,
                item.TransactionGroupID,
                item.RegionalPSNPPlanDetails
            };

            return element;
        }
    }
}
