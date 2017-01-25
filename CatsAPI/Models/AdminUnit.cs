using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class AdminUnit
    {
        public int AdminUnitID { get; set; }
        public int? code { get; set; }
        public string Name { get; set; }
        public string NameAM { get; set; }
        public Nullable<int> AdminUnitTypeID { get; set; }
        public Nullable<int> ParentID { get; set; }

        public AdminUnit(int _adminUnitId, int? _code, string _name, string _nameAM,int? _adminUnitTypeId,int? _parentId)
        {
            AdminUnitID = _adminUnitId;
            code = _code;
            Name = _name;
            NameAM = _nameAM;
            AdminUnitTypeID = _adminUnitTypeId;
            ParentID = _parentId;
        }
    }
}