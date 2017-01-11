using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class Unit
    {
        public int UnitID { get; set; }
        public string Name { get; set; }
        public Unit(int unitId, string name)
        {
            UnitID = unitId;
            Name = name;
        }
    }
}