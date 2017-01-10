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

       private readonly ICommodityService _commodityService;
        


        public CommodityController(ICommodityService commodityService)
        {
            _commodityService = commodityService;
        }
        /// <summary>
        /// Returns List of commodities
        /// </summary>
        /// <returns>List of commodties</returns>
        [System.Web.Http.HttpGet]
        public List<Models.Commodity> Get()
        {
            var c = _commodityService.GetAllCommodity();
            return c.Select(commodity => new Models.Commodity(commodity.CommodityID, commodity.Name, commodity.LongName, commodity.NameAM, commodity.CommodityCode, commodity.CommodityTypeID, commodity.CommodityType.Name, commodity.ParentID, GetCommodityParentName(commodity.ParentID))).ToList();
        }
        /// <summary>
        /// Used to get a single object by commodityId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public Models.Commodity Get(int id)
        {
            var commodity = _commodityService.FindById(id);
            if (commodity!=null)
            {
                var _commodity = new Models.Commodity(commodity.CommodityID, commodity.Name, commodity.LongName,
                                                      commodity.NameAM, commodity.CommodityCode,
                                                      commodity.CommodityTypeID, commodity.CommodityType.Name,
                                                      commodity.ParentID, GetCommodityParentName(commodity.ParentID));
                return _commodity;
                
            }
            return null;
        }

        //public int GetCommodityByName(string name)
        //{
        //    var commodity = _commodityService.FindBy(n => n.Name == name).FirstOrDefault();
        //    return commodity != null ? commodity.CommodityID : -1;
        //}
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
