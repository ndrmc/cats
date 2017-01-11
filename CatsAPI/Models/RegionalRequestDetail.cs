using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class RegionalRequestDetail
    {
        //RegionalRequestDetailId, RegionalRequestId, FdpID, FdpName, Beneficiaries
        public int RegionalRequestDetailId { get; set; }
        public int RegionalRequestId { get; set; }
        public int FdpId { get; set; }
        public string FdpName { get; set; }
        public int Beneficiaries { get; set; }
    }
}