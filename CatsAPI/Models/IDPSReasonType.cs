using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class IDPSReasonType
    {
        public int IDPSId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IDPSReasonType(int idpsId, string name, string description)
        {
            IDPSId = idpsId;
            Name = name;
            Description = description;
        }
    }
}