using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Cats.Services.Administration;
namespace Cats.Rest.Controllers
{
    public class IDPSReasonTypeController : ApiController
    {
        //
        // GET: /IDPSReasonType/

        private IIDPSReasonTypeServices _idpsReasonTypeServices;

        public IDPSReasonTypeController(IIDPSReasonTypeServices idpsReasonTypeServices)
        {
            _idpsReasonTypeServices = idpsReasonTypeServices;
        }
        /// <summary>
        /// Gets all IDPS types
        /// </summary>
        /// <returns></returns>
        public List<Models.IDPSReasonType> Get()
        {
            var IDPSReasonTypes = _idpsReasonTypeServices.GetAllIDPSReasonType();
            return IDPSReasonTypes != null ? IDPSReasonTypes.Select(idpsReasonType => new Models.IDPSReasonType(idpsReasonType.IDPSId, idpsReasonType.Name, idpsReasonType.Description)).ToList() : null;
        }
        /// <summary>
        /// Return a single  object of IDPS type given an id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.IDPSReasonType Get(int id)
        {
            var IDPSReasonType = _idpsReasonTypeServices.FindById(id);
            if (IDPSReasonType !=null)
            {
                var newIDPS = new Models.IDPSReasonType(IDPSReasonType.IDPSId, IDPSReasonType.Name,
                                                        IDPSReasonType.Description);
                return newIDPS;
            }
            return null;
        }
    }
}
