using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class ReliefRequisitionDetail
    {
        public int RequisitionDetailID { get; set; }
        public int RequisitionID { get; set; }
        public int CommodityID { get; set; }
        public string CommodityName { get; set; }
        public int BenficiaryNo { get; set; }
        public decimal Amount { get; set; }
        public int FDPID { get; set; }
        public string FDPName { get; set; }
        public Nullable<int> DonorID { get; set; }
        public string   DonorName { get; set; }
        public decimal? Contingency { get; set; }

        public ReliefRequisitionDetail(int requisitionDetailId, int requisitionId, int commodityId, string commodityName, 
            int beneficiaryNo,decimal amount, int fdpId, string fdpName, int? donorId, string donorNamae,decimal? contigency)
        {
            RequisitionDetailID = requisitionDetailId;
            RequisitionID = requisitionId;
            CommodityID = commodityId;
            CommodityName = commodityName;
            BenficiaryNo = beneficiaryNo;
            Amount = amount;
            FDPID = fdpId;
            FDPName = fdpName;
            DonorID = donorId;
            DonorName = DonorName;
            Contingency = contigency;
        }
    }
}