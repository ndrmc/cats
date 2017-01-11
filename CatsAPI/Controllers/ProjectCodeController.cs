using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Cats.Services.EarlyWarning;

namespace Cats.Rest.Controllers
{
    public class ProjectCodeController : ApiController
    {
        //
        // GET: /ProjectCode/

        public IProjectCodeService _ProjectCodeService;
       public ProjectCodeController(IProjectCodeService projectCodeService)
        {
            _ProjectCodeService = projectCodeService;
        }
        /// <summary>
        /// Gets all Project Codes
        /// </summary>
        /// <returns></returns>
        public List<Models.ProjectCode> Get()
        {
            var projectCodes = _ProjectCodeService.GetAllProjectCode();

            return projectCodes.Select(projectCode => new Models.ProjectCode(projectCode.ProjectCodeID, projectCode.Value)).ToList();
        }
        /// <summary>
        /// Gets a single Project code by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.ProjectCode Get(int id)
        {
            var projectCode = _ProjectCodeService.FindById(id);
            if (projectCode!=null)
            {
                var newProjectCode = new Models.ProjectCode(projectCode.ProjectCodeID, projectCode.Value);
                return newProjectCode;
            }
            return null;
        }

    }
}
