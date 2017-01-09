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
        private readonly IAdminUnitService _adminUnitService;
        public HomeController( ICommodityService commodityService, ICommonService _common, IAdminUnitService adminUnitService)
        {

            _commodityService = commodityService;
            _commonService = _common;
            _adminUnitService = adminUnitService;
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
    }
}