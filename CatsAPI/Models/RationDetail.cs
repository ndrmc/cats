using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class RationDetail
    {
        public int RationDetailID { get; set; }
        public int RationID { get; set; }
        public int CommodityID { get; set; }
        public string CommodityName { get; set; }
        public decimal Amount { get; set; }
        public Nullable<int> UnitID { get; set; }
        public string UnitName { get; set; }

        public RationDetail(int rationDetailId, int rationId, int commodityId, string commodityName, decimal amount,
                            int? unitId, string unitName)
        {
            RationDetailID = rationDetailId;
            RationID = rationId;
            CommodityID = commodityId;
            CommodityName = commodityName;
            Amount = amount;
            UnitID = unitId;
            UnitName = unitName;
        }

    }
}