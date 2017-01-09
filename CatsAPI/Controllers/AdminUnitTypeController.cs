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
        public List<Models.AdminUnitType> Get()
        {
            var adminUnitTypes = _adminUnitTypeService.GetAllAdminUnitType();
            return adminUnitTypes.Select(adminUnitType => new Models.AdminUnitType(adminUnitType.AdminUnitTypeID, adminUnitType.Name, adminUnitType.NameAM)).ToList();
        }
        /// <summary>
        /// returns the name of the Admin unit Type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Get(int id)
        {
            var adminUnitType = _adminUnitTypeService.FindById(id);
            return adminUnitType != null ? adminUnitType.Name : "";
        }
        /// <summary>
        /// Returns the id of an AdminUnitType 
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Id of Admin Unit Type</returns>
        public int GetAdminUnitTypesByName(string name)
        {
            var adminUnitType = _adminUnitTypeService.FindBy(n => n.Name == name).FirstOrDefault();
            return adminUnitType != null ? adminUnitType.AdminUnitTypeID : -1;
        }
        #endregion

    }
}
