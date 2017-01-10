using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Cats.Services.EarlyWarning;

namespace Cats.Rest.Controllers
{
    public class HubController : ApiController

    {
        //
        // GET: /Hub/

        private readonly IHubService _hubService;

        public HubController(IHubService hubService)
        {
            _hubService = hubService;
        }
        /// <summary>
        /// Gets all hubs
        /// </summary>
        /// <returns></returns>
        public List<Models.Hub> Get()
        {
            var hubs = _hubService.GetAllHub();
            return hubs.Select(hub => new Models.Hub(hub.HubID, hub.Name, hub.HubOwnerID, hub.HubOwner.Name, hub.HubParentID, GetParentHubId(hub.HubParentID))).ToList();
        }
        /// <summary>
        /// Gets a single hub by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.Hub Get(int id)
        {
            var hub = _hubService.FindById(id);
            if (hub!=null)
            {
                var newHub = new Models.Hub(hub.HubID, hub.Name, hub.HubOwnerID, hub.HubOwner.Name, hub.HubParentID,
                                            GetParentHubId(hub.HubParentID));
                return newHub;
            }
            return null;
        }
        private string GetParentHubId(int hubParentId)
        {
            var hub = _hubService.FindById(hubParentId);
            return hub != null ? hub.Name : null;
        }
    }
}
