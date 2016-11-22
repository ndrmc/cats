using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace Cats.Models.Hubs.Mapping
{
    public class PartitionMap : EntityTypeConfiguration<Partition>
    {
        public PartitionMap()
        {
            // Primary Key
            this.HasKey(t => t.PartitionId);

            // Properties
            this.Property(t => t.ServerUserName)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Partition");
            this.Property(t => t.PartitionId).HasColumnName("PartitionId");
            this.Property(t => t.HubID).HasColumnName("HubID");
            this.Property(t => t.ServerUserName).HasColumnName("ServerUserName");
            this.Property(t => t.PartitionCreatedDate).HasColumnName("PartitionCreatedDate");
            this.Property(t => t.LastUpdated).HasColumnName("LastUpdated");
            this.Property(t => t.LastSyncTime).HasColumnName("LastSyncTime");
            this.Property(t => t.HasConflict).HasColumnName("HasConflict");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
        }
    }
}
