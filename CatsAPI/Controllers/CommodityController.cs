using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Cats.Services.Common;
using Cats.Services.EarlyWarning;

namespace Cats.Rest.Controllers
{
    public class CommodityController : ApiController

    {
        //
        // GET: /Commody/

       private ICommodityService _commodityService;
        


        public CommodityController(ICommodityService commodityService)
        {
            _commodityService = commodityService;
        }

       
        public List<Models.Commodity> Get()
        {
            var c = _commodityService.GetAllCommodity();
            return c.Select(commodity => new Models.Commodity(commodity.CommodityID, commodity.Name, commodity.LongName, commodity.NameAM, commodity.CommodityCode, commodity.CommodityTypeID, commodity.CommodityType.Name, commodity.ParentID, GetCommodityParentName(commodity.ParentID))).ToList();
        }

        public string Get(int id)
        {
            var commodity = _commodityService.FindById(id);
            return commodity != null ? commodity.Name : null;
        }

        public int GetCommodityByName(string name)
        {
            var commodity = _commodityService.FindBy(n => n.Name == name).FirstOrDefault();
            return commodity != null ? commodity.CommodityID : -1;
        }
        private string GetCommodityParentName(int? ParentId)
        {
            if (ParentId == null) return null;
            var firstOrDefault = _commodityService.FindBy(i => i.ParentID == ParentId).FirstOrDefault();
            if (firstOrDefault != null)
            {
                var name = firstOrDefault.Name;
                return name;
            }
            return null;
        }

    }
}
