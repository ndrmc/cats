using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Cats.Services.EarlyWarning;

namespace Cats.Rest.Controllers
{
    public class AdminUnitTypeController : ApiController
    {
        private readonly IAdminUnitTypeService _adminUnitTypeService;

        public AdminUnitTypeController(IAdminUnitTypeService adminUnitTypeService)
        {
            _adminUnitTypeService = adminUnitTypeService;
        }

        //
        // GET: /AdminUnitType/

        #region Admin Unit Type
        /// <summary>
        /// Returns all admin unit types
        /// </summary>
        /// <returns>List</returns>
        public List<Models.AdminUnitType> GetAdminUnitTypes()
        {
            var adminUnitTypes = _adminUnitTypeService.GetAllAdminUnitType();
            return adminUnitTypes.Select(adminUnitType => new Models.AdminUnitType(adminUnitType.AdminUnitTypeID, adminUnitType.Name, adminUnitType.NameAM)).ToList();
        }
        /// <summary>
        /// Used to get a single object of Admin Unit by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.AdminUnitType GetAdminUnitType(int id)
        {
            var adminUnitType = _adminUnitTypeService.FindById(id);
            if (adminUnitType != null)
            {
                var _adminUnitType = new Models.AdminUnitType(adminUnitType.AdminUnitTypeID, adminUnitType.Name,
                                                              adminUnitType.NameAM);
                return _adminUnitType;
            }
            return null;
           
        }
        /// <summary>
        /// Returns the id of an AdminUnitType 
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Id of Admin Unit Type</returns>
        //public int GetAdminUnitTypesByName(string name)
        //{
        //    var adminUnitType = _adminUnitTypeService.FindBy(n => n.Name == name).FirstOrDefault();
        //    return adminUnitType != null ? adminUnitType.AdminUnitTypeID : -1;
        //}
        #endregion

    }
}
