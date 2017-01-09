using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class AdminUnitType
    {
        public int AdminUnitTypeID { get; set; }
        public string Name { get; set; }
        public string NameAM { get; set; }

        public AdminUnitType(int _AdminUnitTypeId, string _name, string _NameAM)
        {
            AdminUnitTypeID = _AdminUnitTypeId;
            Name = _name;
            NameAM = _NameAM;
        }
    }
}