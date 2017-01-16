using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class SupportType
    {
        public int SupportTypeID { get; set; }
        public string Description { get; set; }

        public SupportType(int supportTypeID, string description)
        {
            SupportTypeID = supportTypeID;
            Description = description;
        }
    }
}