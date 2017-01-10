using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class HRDDetail
    {
        //HRDDetailID, HRDID, Duration, AdminUnitId, WoredaName, Beneficaries, StartingMonth
        public int HRDDetailID { get; set; }
        public int AdminUnitId { get; set; }
        public string WoredaName { get; set; }
        public int Beneficiaries { get; set; }
        public int Duration { get; set; }
        public int StartingMonth { get; set; }
    }
}