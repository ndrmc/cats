using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Cats.Models;
using Cats.Services.Common;
using Cats.Services.EarlyWarning;

namespace Cats.Rest.Controllers
{
    public class HomeController : ApiController
    {
       
        private ICommodityService _commodityService;
        private readonly ICommonService _commonService;
        public HomeController( ICommodityService commodityService, ICommonService _common)
        {

            _commodityService = commodityService;
            _commonService = _common;
        }

        [HttpGet]
        public List<Models.Commodity> GetCommodities()
        {
            var c = _commonService.GetCommodities();
            return c.Select(commodity => new Models.Commodity(commodity.CommodityID, commodity.Name)).ToList();
        }
    }
}