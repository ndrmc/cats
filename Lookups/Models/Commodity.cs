using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class Commodity
    {
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }

        public Commodity(int Id, string Name)
        {
            CommodityId = Id;
            CommodityName = Name;

        }
    }
}