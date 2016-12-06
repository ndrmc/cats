using System;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models.Hubs.Mapping
{
    public class VWReceiptAllocationAggregateMap : EntityTypeConfiguration<VWReceiptAllocationAggregate>
    {
        public VWReceiptAllocationAggregateMap()
        {
            this.HasKey(t => new { t.ReceiptAllocationID });

            // Table & Column Mappings
            this.ToTable("VWReceiptAllocationAggregate");
            this.Property(t => t.ReceiptAllocationID).HasColumnName("ReceiptAllocationID");
            this.Property(t => t.CommodityName).HasColumnName("CommodityName");
            this.Property(t => t.SINumber).HasColumnName("SINumber");
            this.Property(t => t.ProjectNumber).HasColumnName("ProjectNumber");
            this.Property(t => t.ReceivedQuantity).HasColumnName("ReceivedQuantity");
            this.Property(t => t.HubID).HasColumnName("HubID");
            this.Property(t => t.IsClosed).HasColumnName("IsClosed");
            this.Property(t => t.IsFalseGRN).HasColumnName("IsFalseGRN");
            this.Property(t => t.CommodityTypeID).HasColumnName("CommodityTypeID");
            this.Property(t => t.CommoditySourceID).HasColumnName("CommoditySourceID");
            this.Property(t => t.IsCommited).HasColumnName("IsCommited");
            this.Property(t => t.ETA).HasColumnName("ETA");
            this.Property(t => t.GiftCertificateDetailID).HasColumnName("GiftCertificateDetailID");
            this.Property(t => t.CommodityID).HasColumnName("CommodityID");
            this.Property(t => t.UnitID).HasColumnName("UnitID");
            this.Property(t => t.QuantityInUnit).HasColumnName("QuantityInUnit");
            this.Property(t => t.QuantityInMT).HasColumnName("QuantityInMT");
            this.Property(t => t.DonorID).HasColumnName("DonorID");
            this.Property(t => t.ProgramID).HasColumnName("ProgramID");
            this.Property(t => t.PurchaseOrder).HasColumnName("PurchaseOrder");
            this.Property(t => t.SupplierName).HasColumnName("SupplierName");
            this.Property(t => t.SourceHubID).HasColumnName("SourceHubID");
            this.Property(t => t.OtherDocumentationRef).HasColumnName("OtherDocumentationRef");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.BusinessProcessID).HasColumnName("BusinessProcessID");

        }
    }
}
