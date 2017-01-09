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
        private readonly IAdminUnitService _adminUnitService;
        private readonly IAdminUnitTypeService _adminUnitTypeService;
        public HomeController( ICommodityService commodityService, ICommonService _common, IAdminUnitService adminUnitService, IAdminUnitTypeService adminUnitTypeService)
        {

            _commodityService = commodityService;
            _commonService = _common;
            _adminUnitService = adminUnitService;
            _adminUnitTypeService = adminUnitTypeService;
        }


      [HttpGet]
        public List<Models.Commodity> GetCommodities()
        {
            var c = _commonService.GetCommodities();
            return c.Select(commodity => new Models.Commodity(commodity.CommodityID, commodity.Name)).ToList();
        }

      #region AdminUnits
     

      [HttpGet]
        public List<Models.AdminUnit> GetAdminUnit()
        {
            var adminUnits = _adminUnitService.GetAllAdminUnit().ToList();
         
            return adminUnits.Select(adminUnit => adminUnit.ParentID != null ? new Models.AdminUnit(adminUnit.AdminUnitID, adminUnit.AdminUnitID, adminUnit.Name, adminUnit.NameAM, adminUnit.AdminUnitID, (int)adminUnit.ParentID) : null).ToList();
        }
        [HttpGet]
        public string GetAdminUnitById(int id=1)
        {
            var adminUnit = _adminUnitService.FindById(id);
            return adminUnit != null ? adminUnit.Name : "";
        }
        [HttpGet]
        public int GetAdminUnitByName(string name="Afar")
        {
            var adminUnit = _adminUnitService.FindBy(n => n.Name == name).SingleOrDefault();
            return adminUnit != null ? adminUnit.AdminUnitID : -1;
        }
      #endregion

      #region Admin Unit Type

        public List<Models.AdminUnitType> GetAdminUnitTypes()
        {
            var adminUnitTypes = _adminUnitTypeService.GetAllAdminUnitType();
            return adminUnitTypes.Select(adminUnitType => new Models.AdminUnitType(adminUnitType.AdminUnitTypeID, adminUnitType.Name, adminUnitType.NameAM)).ToList();
        }

        public string GetAdminUnitTypesById(int id)
        {
            var adminUnitType = _adminUnitTypeService.FindById(id);
            return adminUnitType != null ? adminUnitType.Name : "";
        }

        public int GetAdminUnitTypesByName(string name)
        {
            var adminUnitType = _adminUnitTypeService.FindBy(n => n.Name == name).FirstOrDefault();
            return adminUnitType != null ? adminUnitType.AdminUnitTypeID : -1;
        }
        #endregion
    }
}