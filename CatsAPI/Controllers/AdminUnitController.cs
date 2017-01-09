using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Cats.Services.EarlyWarning;

namespace Cats.Rest.Controllers
{
    public class AdminUnitController : ApiController
    {
        private readonly IAdminUnitService _adminUnitService;
        //
        // GET: /AdminUnit/
      

        public AdminUnitController(IAdminUnitService adminUnitService)
        {
            _adminUnitService = adminUnitService;
        }

        [System.Web.Mvc.HttpGet]
        public  List<Models.AdminUnit> Get()
        {
            var adminUnits = _adminUnitService.GetAllAdminUnit().ToList();

            return adminUnits.Select(adminUnit => adminUnit.ParentID != null ? new Models.AdminUnit(adminUnit.AdminUnitID, adminUnit.AdminUnitID, adminUnit.Name, adminUnit.NameAM, adminUnit.AdminUnitID, (int)adminUnit.ParentID) : null).ToList();
        }
      
        [System.Web.Http.HttpGet]
        public string Get(int id)
        {
            var adminUnit = _adminUnitService.FindById(id);
            return adminUnit != null ? adminUnit.Name : "";
        }

    }
}
