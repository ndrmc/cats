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
      ///Returns a single object given an Id
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
        [System.Web.Http.HttpGet]
        public Models.AdminUnit GetAdminUnit(int id)
        {
            var adminUnit = _adminUnitService.FindById(id);
           if (adminUnit!=null)
           {
               if (adminUnit.AdminUnitTypeID != null)
               {
                   var _adminUnit = new Models.AdminUnit(adminUnit.AdminUnitID, adminUnit.code, adminUnit.Name,
                                                         adminUnit.NameAM, adminUnit.AdminUnitTypeID, adminUnit.ParentID);
                   return _adminUnit;
               }
           }
          return null;
        }
        /// <summary>
        /// Returns list of admin units given an admin unit type id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Models.AdminUnit> GetAdminUnitsByAdminUnitType(int id)
        {
            var adminUnits = _adminUnitService.FindBy(t=>t.AdminUnitTypeID ==id).ToList();
            return adminUnits.Select(adminUnit => new Models.AdminUnit(adminUnit.AdminUnitID,adminUnit.code, adminUnit.Name, adminUnit.NameAM, adminUnit.AdminUnitTypeID, adminUnit.ParentID)).ToList();
        } 

        /// <summary>
        /// Returns a list of admin Units under a given admin unit id - a region id, zone id or  a woreda id.
        /// </summary>
        /// <param name="id">region id, zone id, or woreda id</param>
        /// <returns>admin units</returns>
        public List<Models.AdminUnit> GetAdminUnitDetails(int id)
        {
            var adminUnits = _adminUnitService.FindBy(r=>r.ParentID == id).ToList();

            return adminUnits.Select(adminUnit => adminUnit.ParentID != null ? new Models.AdminUnit(adminUnit.AdminUnitID, adminUnit.AdminUnitID, adminUnit.Name, adminUnit.NameAM, adminUnit.AdminUnitID, (int)adminUnit.ParentID) : null).ToList(); 
        }
    }
}
