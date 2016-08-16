using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models.Hubs.Mapping
{
    public class VWDispatchAllocationMap : EntityTypeConfiguration<VWDispatchAllocation>
    {
        public VWDispatchAllocationMap()
        {
            // Primary Key
            this.HasKey(t => t.DispatchAllocationID);

            // Properties
            //this.Property(t => t.RequisitionNo)
            //    .IsRequired()
            //    .HasMaxLength(50);

            //this.Property(t => t.BidRefNo)
            //    .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VWDispatchAllocation");
            this.Property(t => t.DispatchAllocationID).HasColumnName("DispatchAllocationID");
            this.Property(t => t.HubID).HasColumnName("HubID");
            this.Property(t => t.StoreID).HasColumnName("StoreID");
            this.Property(t => t.Year).HasColumnName("Year");
            this.Property(t => t.Month).HasColumnName("Month");
            this.Property(t => t.Round).HasColumnName("Round");
            this.Property(t => t.DonorID).HasColumnName("DonorID");
            this.Property(t => t.ProgramID).HasColumnName("ProgramID");
            this.Property(t => t.CommodityID).HasColumnName("CommodityID");
            this.Property(t => t.CommodityName).HasColumnName("CommodityName");
            this.Property(t => t.CommodityTypeID).HasColumnName("CommodityTypeID");
            this.Property(t => t.RequisitionNo).HasColumnName("RequisitionNo");
            this.Property(t => t.RequisitionId).HasColumnName("RequisitionId");
            this.Property(t => t.BidRefNo).HasColumnName("BidRefNo");
            this.Property(t => t.ContractStartDate).HasColumnName("ContractStartDate");
            this.Property(t => t.ContractEndDate).HasColumnName("ContractEndDate");
            this.Property(t => t.Beneficiery).HasColumnName("Beneficiery");
            this.Property(t => t.Amount).HasColumnName("Amount").HasPrecision(18, 4);
            this.Property(t => t.Unit).HasColumnName("Unit");
            this.Property(t => t.TransporterID).HasColumnName("TransporterID");
            this.Property(t => t.TransporterName).HasColumnName("TransporterName");
            this.Property(t => t.FDPID).HasColumnName("FDPID");
            this.Property(t => t.FDPName).HasColumnName("FDPName");
            this.Property(t => t.WoredaID).HasColumnName("WoredaID");
            this.Property(t => t.WoredaName).HasColumnName("WoredaName");
            this.Property(t => t.ZoneID).HasColumnName("ZoneID");
            this.Property(t => t.ZoneName).HasColumnName("ZoneName");
            this.Property(t => t.RegionID).HasColumnName("RegionID");
            this.Property(t => t.RegionName).HasColumnName("RegionName");
            this.Property(t => t.ShippingInstructionID).HasColumnName("ShippingInstructionID");
            this.Property(t => t.ProjectCodeID).HasColumnName("ProjectCodeID");
            this.Property(t => t.TransportOrderID).HasColumnName("TransportOrderID");
            this.Property(t => t.IsClosed).HasColumnName("IsClosed");
            this.Property(t => t.DispatchedAmount).HasColumnName("DispatchedAmount");
            this.Property(t => t.DispatchedAmountInUnit).HasColumnName("DispatchedAmountInUnit");
        

        }
    }
}
