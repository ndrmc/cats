using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models.Hubs.Mapping
{
    public class VWDdispatchAllocationDistributionMap : EntityTypeConfiguration<VWDdispatchAllocationDistribution>
    {
        public VWDdispatchAllocationDistributionMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Transporter, t.Commodity, t.DispatchDate, t.AllocatedAmount, t.DispatchedAmount, t.Diff1});

            // Properties
            this.Property(t => t.Transporter);

            this.Property(t => t.Commodity)
                .HasMaxLength(50);

            //this.Property(t => t.Hub)
            //    .HasMaxLength(50);

            //this.Property(t => t.Fdp)
            //    .HasMaxLength(50);

            this.Property(t => t.DispatchDate);

            this.Property(t => t.AllocatedAmount);

            this.Property(t => t.DispatchedAmount);

            this.Property(t => t.Diff1);

            //this.Property(t => t.Diff2);

            // Table & Column Mappings
            this.ToTable("VWDispatchAllocationDistribution");

            this.Property(t => t.Transporter).HasColumnName("Transporter");
            this.Property(t => t.Commodity).HasColumnName("Commodity");
            //this.Property(t => t.Hub).HasColumnName("Hub");
            //this.Property(t => t.Fdp).HasColumnName("FDP");
            this.Property(t => t.DispatchDate).HasColumnName("DispatchDate");
            this.Property(t => t.AllocatedAmount).HasColumnName("AllocatedAmount");
            //this.Property(t => t.Hub).HasColumnName("Hub");
            this.Property(t => t.Commodity).HasColumnName("Commodity");
            this.Property(t => t.DispatchedAmount).HasColumnName("DispatchedAmount");
            this.Property(t => t.Diff1).HasColumnName("Diff1");
            //this.Property(t => t.Diff2).HasColumnName("Diff2");
        }
    }
}
