using System.Data.Entity.ModelConfiguration;

namespace Cats.Models.Shared.DashBoardModels
{
    public class DashboardDataEntryMap : EntityTypeConfiguration<DashboardDataEntry>
    {
        public DashboardDataEntryMap()
        {
            this.Property(t => t.UserName).HasColumnName("PerformedBy");
            this.Property(t => t.ActivityName).HasColumnName("ActivityName");
            this.Property(t => t.ActivityCount).HasColumnName("ActivityCount");
        }
    }
}
