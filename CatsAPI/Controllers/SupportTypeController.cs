using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Cats.Services.Logistics;

namespace Cats.Rest.Controllers
{
    public class SupportTypeController : ApiController
    {
        //
        // GET: /SupportType/

        private ISupportTypeService _supporTypeService;

        public SupportTypeController(ISupportTypeService supportTypeService)
        {
            _supporTypeService = supportTypeService;
        }
        /// <summary>
        /// Gets all support Types
        /// </summary>
        /// <returns></returns>
        public List<Models.SupportType> Get()
        {
            var supportTypes = _supporTypeService.GetAllSupportType();
            if (supportTypes != null)
                return supportTypes.Select(supportType => new Models.SupportType(supportType.SupportTypeID, supportType.Description)).ToList();
            return null;
        }

        /// <summary>
        /// Returns a single object of support type given a support id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.SupportType Get(int id)
        {
            var supportType = _supporTypeService.FindById(id);
            if (supportType != null)
            {
                var newSupportType = new Models.SupportType(supportType.SupportTypeID, supportType.Description);
                return newSupportType;
            }

            return null;
        }

    }
}
