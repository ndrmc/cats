using Cats.Services.EarlyWarning;
using System.Linq;
using System.Web.Http;

namespace Cats.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class RationDetailController : ApiController
    {
        private readonly IRationDetailService _irationDetailService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="irationDetailService"></param>
        public RationDetailController(IRationDetailService irationDetailService)
        {
            _irationDetailService = irationDetailService;
        }
        /// <summary>
        /// Returns list of RagionDetail objects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetRagionDetails()
        {
            var bids = _irationDetailService.GetAllRationDetail().Select(item => new
            {
                item.RationDetailID,
                item.RationID,
                item.CommodityID,
                CommodityName = item.Commodity.Name,
                item.Amount,
                item.UnitID,
                UnitName = item.Unit.Name
            }).ToList();

            return bids;
        }
        /// <summary>
        /// Given an id returns a RagionDetail object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetRagionDetail(int id)
        {
            var obj = _irationDetailService.FindById(id);
            var element = new
            {
                obj.RationDetailID,
                obj.RationID,
                obj.CommodityID,
                CommodityName = obj.Commodity.Name,
                obj.Amount,
                obj.UnitID,
                UnitName = obj.Unit.Name
            };

            return element;
        }
    }
}
