using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Cats.Models.Hubs.Mapping
{
    public class UserHubMap : EntityTypeConfiguration<UserHub>
    {
        public UserHubMap()
        {  // Primary Key
            //this.HasKey(t => new { t.UserProfileID, t.HubID });

            this.HasKey(t => t.UserHubID);
            // Properties
            this.Property(t => t.UserHubID);
              //  .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.UserProfileID);
               // .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.HubID);
              //  .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.IsDefault)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("UserHub");
            this.Property(t => t.UserHubID).HasColumnName("UserHubID");
            this.Property(t => t.UserProfileID).HasColumnName("UserProfileID");
            this.Property(t => t.HubID).HasColumnName("HubID");
            this.Property(t => t.IsDefault).HasColumnName("IsDefault");

            // Relationships
            this.HasRequired(t => t.Hub)
                .WithMany(t => t.UserHubs)
                .HasForeignKey(d => d.HubID);
            this.HasRequired(t => t.UserProfile)
                .WithMany(t => t.UserHubs)
                .HasForeignKey(d => d.UserProfileID);
   
        }
    }
}
