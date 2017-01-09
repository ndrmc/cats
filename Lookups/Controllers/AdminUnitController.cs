using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cats.Rest.Models;
using Cats.Services.EarlyWarning;
using Cats.Models;

namespace Cats.Rest.Controllers
{
    public class AdminUnitController : ApiController
    {
        private readonly IAdminUnitService _adminUnitService;
        public AdminUnitController(IAdminUnitService adminUnitservice)
        {
            _adminUnitService = adminUnitservice;
        }
        [HttpGet]
        public IEnumerable<Models.AdminUnit> Get()
        {
            var c = _adminUnitService.GetAllAdminUnit().OrderBy(o => o.Name);
            //Id, Name, NameAM, AdminUnitTypeId, AdminUnitTypeName, ParentId, Code
            return c.Select(admin => new Rest.Models.AdminUnit(admin.AdminUnitID, admin.code ?? 0, admin.Name, admin.NameAM, admin.AdminUnitTypeID ?? 0, admin.ParentID ?? 0, admin.AdminUnitType.Name)).ToList();
        }
    }
},