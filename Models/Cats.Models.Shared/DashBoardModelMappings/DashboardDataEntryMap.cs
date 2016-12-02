using System.Data.Entity.ModelConfiguration;

namespace Cats.Models.Shared.DashBoardModels
{
    public class DashboardDataEntryMap : EntityTypeConfiguration<DashboardDataEntry>
    {
        public DashboardDataEntryMap()
        {
            HasKey(t => new { t.ActivityName, t.PerformedBy, t.SettingName });

            //Property(t => t.Row).HasColumnName("Row");
            Property(t => t.PerformedBy).HasColumnName("PerformedBy");
            Property(t => t.SettingName).HasColumnName("SettingName");
            Property(t => t.ActivityName).HasColumnName("ActivityName");
            Property(t => t.ActivityCount).HasColumnName("ActivityCount");
        }
    }
}
