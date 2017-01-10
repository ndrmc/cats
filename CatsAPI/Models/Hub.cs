using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class Hub
    {
        public int HubID { get; set; }
        public string Name { get; set; }
        public int HubOwnerID { get; set; }
        public string  HubOwner { get; set; }
        public int? HubParentId { get; set; }
        public string ParentHub { get; set; }

        public Hub(int hubId, string name, int hubOwnerId, string hubOwner, int? hubParentId, string parentHub)
        {
            HubID = hubId;
            Name = name;
            HubOwnerID = hubOwnerId;
            HubOwner = HubOwner;
            HubParentId = hubParentId;
            ParentHub = parentHub;
        }
    }
}