using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Cats.Rest.Models;
using Cats.Services.EarlyWarning;
namespace Cats.Rest.Controllers
{
    public class CommodityTypeController : ApiController
    {
        private readonly ICommodityTypeService _commodityTypeService;
        public CommodityTypeController(ICommodityTypeService commodityTypeService)
        {
            _commodityTypeService = commodityTypeService;
        }
        [HttpGet]
        public List<CommodityType> GetCommodityTypes()
        {
            return (from c in _commodityTypeService.GetAllCommodityType()
                    select new CommodityType()
                    {
                        Id = c.CommodityTypeID,
                        Name = c.Name
                    }).ToList();
        }
        [HttpGet]
        public List<CommodityType> GetCommodityType(int id)
        {
            return (from c in _commodityTypeService.GetAllCommodityType().Where(c => c.CommodityTypeID == id)
                    select new CommodityType()
                    {
                        Id = c.CommodityTypeID,
                        Name = c.Name
                    }).ToList();
        }
    }
}