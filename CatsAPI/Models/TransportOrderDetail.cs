using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class TransportOrderDetail
    {
        public int TransportOrderDetailID { get; set; }
        public int TransportOrderID { get; set; }
        public int FdpID { get; set; }
        public string FdpName { get; set; }
        public int SourceWarehouseID { get; set; }
        public string SourceWarehouseName { get; set; }
        public decimal QuantityQuintal { get; set; }
        public Nullable<decimal> DistanceFromOrigin { get; set; }
        public decimal TariffPerQuintal { get; set; }
        public int RequisitionID { get; set; }
        public string RequisitionNo { get; set; }
        public int CommodityID { get; set; }
        public string CommodityName { get; set; }
        public Nullable<int> ZoneID { get; set; }
        public string ZoneName { get; set; }
        public Nullable<int> DonorID { get; set; }
        public string DonorNamae { get; set; }
        public int? BidID { get; set; }
        public bool IsChanged { get; set; }
        public bool? WinnerAssignedByLogistics { get; set; }
    }
}