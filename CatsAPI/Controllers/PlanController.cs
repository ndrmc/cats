using Cats.Services.EarlyWarning;
using System.Linq;
using System.Web.Http;

namespace Cats.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class PlanController : ApiController
    {
        private readonly IPlanService _iplanService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iplanService"></param>
        public PlanController(IPlanService iplanService)
        {
            _iplanService = iplanService;
        }
        /// <summary>
        /// Returns list of Plan objects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetPlans()
        {
            var bids = _iplanService.GetAllPlan().Select(item => new
            {
                item.PlanID,
                item.PlanName,
                item.StartDate,
                item.EndDate,
                item.ProgramID,
                item.Program,
                item.Status,
                item.Duration
            }).ToList();

            return bids;
        }
        /// <summary>
        /// Given an id returns a Plan object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetBid(int id)
        {
            var obj = _iplanService.FindById(id);
            var element = new
            {
                obj.PlanID,
                obj.PlanName,
                obj.StartDate,
                obj.EndDate,
                obj.ProgramID,
                obj.Program,
                obj.Status,
                obj.Duration
            };

            return element;
        }
    }
}
