using System.Linq;
using System.Web.Http;
using Cats.Services.Hub;


namespace Cats.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CommoditySourceController : ApiController
    {
        private readonly ICommoditySourceService _icommoditySourceService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="icommoditySourceService"></param>
        public CommoditySourceController(ICommoditySourceService icommoditySourceService)
        {
            _icommoditySourceService = icommoditySourceService;
        }

        /// <summary>
        /// Returns list of CommoditySource objects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetCommoditySources()
        {
            var bids = _icommoditySourceService.GetAllCommoditySource().Select(item => new
            {
              item.CommoditySourceID,
              item.Name
            }).ToList();

            return bids;
        }
        /// <summary>
        /// Given an id returns a CommoditySource object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetCommoditySource(int id)
        {
            var obj = _icommoditySourceService.FindById(id);
            var element = new
            {
                obj.CommoditySourceID,
                obj.Name
            };

            return element;
        }
    }
}
