using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Cats.Models;
using Cats.Services.Common;
using Cats.Services.EarlyWarning;
using AdminUnitType = Cats.Rest.Models.AdminUnitType;

namespace Cats.Rest.Controllers
{

    public class HomeController : ApiController
    {
       
        private ICommodityService _commodityService;
        private readonly ICommonService _commonService;
     
        private readonly IAdminUnitTypeService _adminUnitTypeService;
        public HomeController( ICommodityService commodityService, ICommonService _common, IAdminUnitTypeService adminUnitTypeService)
        {

            _commodityService = commodityService;
            _commonService = _common;
          
            _adminUnitTypeService = adminUnitTypeService;
        }

       
      [HttpGet]
        public List<Models.Commodity> Get()
        {
            var c = _commonService.GetCommodities();
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
            var firstOrDefault = _commonService.FindBy(i => i.ParentID == ParentId).FirstOrDefault();
            if (firstOrDefault != null)
            {
                var name = firstOrDefault.Name;
                return name;
            }
            return null;
        }


     

      //#region Admin Unit Type

      //  public List<Models.AdminUnitType> GetAdminUnitTypes()
      //  {
      //      var adminUnitTypes = _adminUnitTypeService.GetAllAdminUnitType();
      //      return adminUnitTypes.Select(adminUnitType => new Models.AdminUnitType(adminUnitType.AdminUnitTypeID, adminUnitType.Name, adminUnitType.NameAM)).ToList();
      //  }

      //  public string GetAdminUnitTypesById(int id)
      //  {
      //      var adminUnitType = _adminUnitTypeService.FindById(id);
      //      return adminUnitType != null ? adminUnitType.Name : "";
      //  }

      //  public int GetAdminUnitTypesByName(string name)
      //  {
      //      var adminUnitType = _adminUnitTypeService.FindBy(n => n.Name == name).FirstOrDefault();
      //      return adminUnitType != null ? adminUnitType.AdminUnitTypeID : -1;
      //  }
      //  #endregion
    }
}