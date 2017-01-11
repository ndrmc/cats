using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Cats.Services.EarlyWarning;

namespace Cats.Rest.Controllers
{
    public class UnitController : ApiController
    {
        //
        // GET: /Unit/
        private readonly IUnitService _unitService;

        public UnitController(IUnitService unitService)
        {
            _unitService = unitService;
        }
        /// <summary>
        /// Gets all Units
        /// </summary>
        /// <returns></returns>
        public List<Models.Unit> Get()
        {
            var units = _unitService.GetAllUnit();
            if (units != null) return units.Select(unit => new Models.Unit(unit.UnitID, unit.Name)).ToList();
            return null;
        }
        /// <summary>
        /// Gets a single unit by unit Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.Unit Get(int id)
        {
            var unit = _unitService.FindById(id);
            if (unit !=null)
            {
                var newUnit = new Models.Unit(unit.UnitID, unit.Name);
                return newUnit;
            }
            return null;
        }
    }
}
