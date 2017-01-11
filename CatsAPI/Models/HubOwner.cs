using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class HubOwner
    {
        public int HubOwnerID { get; set; }
        public string Name { get; set; }
        public string LongName { get; set; }
    }
}