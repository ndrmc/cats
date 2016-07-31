using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models.Hubs
{
    public class VWReceiptAllocationAggregate
    {
        
        public decimal? ReceivedQuantity { get; set; }
        public System.Guid ReceiptAllocationID { get; set; }
        public bool IsCommited { get; set; }
        public System.DateTime ETA { get; set; }
        public string ProjectNumber { get; set; }
        public string CommodityName { get; set; }
        public Nullable<int> GiftCertificateDetailID { get; set; }
        public int CommodityID { get; set; }
        public int CommodityTypeID { get; set; }
        public string SINumber { get; set; }
        public Nullable<int> UnitID { get; set; }
        public Nullable<decimal> QuantityInUnit { get; set; }
        public decimal QuantityInMT { get; set; }
        public int HubID { get; set; }
        public Nullable<int> DonorID { get; set; }
        public int ProgramID { get; set; }
        public int CommoditySourceID { get; set; }
        public bool IsClosed { get; set; }
        public string PurchaseOrder { get; set; }
        public string SupplierName { get; set; }
        public Nullable<int> SourceHubID { get; set; }
        public string OtherDocumentationRef { get; set; }
        public bool IsFalseGRN { get; set; }
        public string Remark { get; set; }
    }
}
