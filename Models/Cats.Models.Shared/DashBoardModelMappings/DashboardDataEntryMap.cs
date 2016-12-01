using System.Data.Entity.ModelConfiguration;

namespace Cats.Models.Shared.DashBoardModels
{
    public class DashboardDataEntryMap : EntityTypeConfiguration<DashboardDataEntry>
    {
        public DashboardDataEntryMap()
        {
            Property(t => t.UserName).HasColumnName("PerformedBy");
            Property(t => t.WorkflowDefinition).HasColumnName("SettingName");
            Property(t => t.ActivityName).HasColumnName("ActivityName");
            Property(t => t.ActivityCount).HasColumnName("ActivityCount");
        }
    }
}
