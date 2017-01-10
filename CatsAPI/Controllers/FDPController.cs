using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Cats.Services.EarlyWarning;

namespace Cats.Rest.Controllers
{
    public class FDPController : ApiController
    {
        //
        // GET: /FDP/

        private readonly IFDPService _ifdpService;

        public FDPController(IFDPService ifdpService)
        {
            _ifdpService = ifdpService;
        }
        /// <summary>
        /// Get all FDPs
        /// </summary>
        /// <returns></returns>
         [System.Web.Http.HttpGet] 
        public List<Models.FDP> Get()
        {
            var fdpList = _ifdpService.GetAllFDP();
            if (fdpList != null)
                return fdpList.Select(fdp => new Models.FDP(fdp.FDPID, fdp.Name, fdp.NameAM, fdp.AdminUnitID, fdp.Latitude, fdp.Longitude, fdp.HubID)).ToList();
            return null;
        }
        /// <summary>
        /// Gets a single FDP by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [System.Web.Http.HttpGet] 
        public Models.FDP Get(int id)
        {
            var fdp = _ifdpService.FindById(id);
            if (fdp!=null)
            {
                var newFDP = new Models.FDP(fdp.FDPID, fdp.Name, fdp.NameAM, fdp.AdminUnitID, fdp.Latitude,
                                            fdp.Longitude, fdp.HubID);
                return newFDP;
            }
            return null;

        }
        // [System.Web.Http.HttpGet] 
        //public List<Models.FDP> GetByWoreda(int woredaId)
        //{
        //    var fdpList = _ifdpService.FindBy(w => w.AdminUnitID == woredaId).ToList();
        //    return fdpList.Select(fdp => new Models.FDP(fdp.FDPID, fdp.Name, fdp.NameAM, fdp.AdminUnitID, fdp.Latitude, fdp.Longitude, fdp.HubID)).ToList();
            
        //}
        
    }
}
