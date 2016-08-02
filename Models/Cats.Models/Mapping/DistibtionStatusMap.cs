using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models.Mapping
{
    public class DistibtionStatusMap : EntityTypeConfiguration<DistibtionStatus>
    {
        public DistibtionStatusMap()
        {
            // Primary Key
            this.HasKey(t => new { t.WoredaID });

            

            //this.Property(t => t.WoredaName)
            //    .HasMaxLength(50);

            //this.Property(t => t.WoredaID);

            //this.Property(t => t.FDPID);

            //this.Property(t => t.PlanName)
            //    .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("WoredaDistributionDashboardView");
            this.Property(t => t.PlanName).HasColumnName("PlanName");
            this.Property(t => t.WoredaName).HasColumnName("WoredaName");
            this.Property(t => t.RegionName).HasColumnName("RegionName");
            //this.Property(t => t.FDPID).HasColumnName("FDPID");
            this.Property(t => t.FdpCount).HasColumnName("FdpCount");
            this.Property(t => t.RegionID).HasColumnName("RegionID");
          
            this.Property(t => t.WoredaID).HasColumnName("WoredaID");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.DistributionDate).HasColumnName("DistributionDate");
          
           // this.Property(t => t.FDPName).HasColumnName("FDPName");
           
        }
    }
}
