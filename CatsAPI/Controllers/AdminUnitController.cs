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
        /// <summary>
        /// Return all list of Admin units
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet]
        public  List<Models.AdminUnit> GetAdminUnits()
        {
            var adminUnits = _adminUnitService.GetAllAdminUnit().ToList();

            return adminUnits.Select(adminUnit => adminUnit.ParentID != null ? new Models.AdminUnit(adminUnit.AdminUnitID, adminUnit.AdminUnitID, adminUnit.Name, adminUnit.NameAM, adminUnit.AdminUnitID, (int)adminUnit.ParentID) : null).ToList();
        }
      /// <summary>
      /// Used to get a single  object by Id
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
        [System.Web.Http.HttpGet]
        public Models.AdminUnit GetAdminUnit(int id)
        {
            var adminUnit = _adminUnitService.FindById(id);
           if (adminUnit!=null)
           {
               var _adminUnit = new Models.AdminUnit(adminUnit.AdminUnitID, adminUnit.AdminUnitID, adminUnit.Name,
                                                     adminUnit.NameAM, adminUnit.AdminUnitID, adminUnit.ParentID);
               return _adminUnit;
           }
          return null;
        }

    }
}
