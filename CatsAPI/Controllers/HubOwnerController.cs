using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cats.Services.Hub;
using Cats.Rest.Models;

namespace Cats.Rest.Controllers
{
    public class HubOwnerController : ApiController
    {
        private readonly IHubOwnerService _hubOwnerService;
        public HubOwnerController(IHubOwnerService hubOwnerService)
        {
            _hubOwnerService = hubOwnerService;
        }
        // GET api/<controller>
        public IEnumerable<HubOwner> GetHubOwners()
        {
            return (from h in _hubOwnerService.GetAllHubOwner()
                    select new Models.HubOwner()
                    {
                        HubOwnerID = h.HubOwnerID,
                        LongName = h.LongName,
                        Name = h.Name
                    }).ToList();
        }

        // GET api/<controller>/5
        public HubOwner GetHubOwner(int id)
        {
            var h = _hubOwnerService.FindById(id);
            if (h == null) return null;
            return new Models.HubOwner()
            {
                HubOwnerID = h.HubOwnerID,
                LongName = h.LongName,
                Name = h.Name
            };
          
        }

    }
}