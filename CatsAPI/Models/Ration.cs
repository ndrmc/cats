using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class Ration
    {
        public int RationID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public bool? IsDefaultRation { get; set; }
        public string RefrenceNumber { get; set; }
        public List<RationDetail> RationDetail { get; set; }
    }
}