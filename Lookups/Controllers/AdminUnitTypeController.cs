using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cats.Services.EarlyWarning;
using  Cats.Rest.Models;
namespace Cats.Rest.Controllers
{
    public class AdminUnitTypeController : ApiController
    {
        private readonly IAdminUnitTypeService _adminUnitTypeService;
        public AdminUnitTypeController(IAdminUnitTypeService adminUnitTypeService)
        {
            _adminUnitTypeService = adminUnitTypeService;
        }
        [HttpGet]
        public IList<AdminUnitType> Get()
        {
            return (from a in _adminUnitTypeService.GetAllAdminUnitType()
                    select new AdminUnitType(a.AdminUnitTypeID, a.Name, a.NameAM)).ToList();
        }

    }
}