using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class CommodityGrade
    {
        //CommodityGradeId, Name, Description
        public int CommodityGradeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}