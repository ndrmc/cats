using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models.Hubs
{
    public class VWDispatchAllocation
    {
        public System.Guid DispatchAllocationID { get; set; }
        public int HubID { get; set; }
        public Nullable<int> StoreID { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<int> Round { get; set; }
        public Nullable<int> DonorID { get; set; }
        public Nullable<int> ProgramID { get; set; }
        public int CommodityID { get; set; }
        public string CommodityName { get; set; }
        public int CommodityTypeID { get; set; }
        public string RequisitionNo { get; set; }
        public Nullable<int> RequisitionId { get; set; }
        public string BidRefNo { get; set; }
        public Nullable<System.DateTime> ContractStartDate { get; set; }
        public Nullable<System.DateTime> ContractEndDate { get; set; }
        public Nullable<int> Beneficiery { get; set; }
        public decimal Amount { get; set; }
        public int Unit { get; set; }
        public Nullable<int> TransporterID { get; set; }
        public string TransporterName { get; set; }
        public int FDPID { get; set; }
        public string FDPName { get; set; }
        public int WoredaID { get; set; }
        public string WoredaName { get; set; }
        public int ZoneID { get; set; }
        public string ZoneName { get; set; }
        public int RegionID { get; set; }
        public string RegionName { get; set; }
        public Nullable<int> ShippingInstructionID { get; set; }
        public Nullable<int> ProjectCodeID { get; set; }
        public Nullable<int> TransportOrderID { get; set; }

        public bool IsClosed { get; set; }

        public decimal? DispatchedAmount { get; set; }

        public decimal? DispatchedAmountInUnit { get; set; }
    }
}
