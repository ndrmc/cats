using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class Commodity
    {

        public int CommodityID { get; set; }
        public string Name { get; set; }
        public string LongName { get; set; }
        public string NameAM { get; set; }
        public string CommodityCode { get; set; }
        public int CommodityTypeID { get; set; }
        public string CommodityTypeName { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string ParentCommodityName { get; set; }

        public Commodity(int _commodityId, string _name, string _longName, string _NameAM, string _commodityCode, int _commodtyTypeId, string _commodityTypeName, int? _parentId, string _parentCommodityName)
        {
            CommodityID = _commodityId;
            Name = _name;
            LongName = _longName;
            NameAM = _NameAM;
            CommodityTypeID = _commodtyTypeId;
            CommodityTypeName = _commodityTypeName;
            ParentID = _parentId;
            ParentCommodityName = _parentCommodityName;


        }
    }
}