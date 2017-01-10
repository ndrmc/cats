using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Cats.Services.EarlyWarning;

namespace Cats.Rest.Controllers
{
    public class ProgramController : ApiController
    {
        //
        // GET: /Program/

        public IProgramService _ProgramService;

        public ProgramController(IProgramService programService)
        {
            _ProgramService = programService;
        }
        /// <summary>
        /// Get all programmes
        /// </summary>
        /// <returns></returns>
        public List<Models.Program> Get()
        {
            var programs = _ProgramService.GetAllProgram();
            return programs.Select(program => new Models.Program(program.ProgramID, program.Name, program.Description, program.LongName, program.ShortCode)).ToList();
        }
        /// <summary>
        /// Get a single program by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.Program Get(int id)
        {
            var program = _ProgramService.FindById(id);
            if (program!=null)
            {
                var newProgram = new Models.Program(program.ProgramID, program.Name, program.Description,
                                                    program.LongName, program.ShortCode);

                return newProgram;
            }
            return null;
        }
    }
}
