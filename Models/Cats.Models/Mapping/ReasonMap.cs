using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

namespace Cats.Models.Mapping
{
   
    public class ReasonMap : EntityTypeConfiguration<Reason>
    {

        public ReasonMap()
        {
            // Primary Key
            this.HasKey(t => t.ReasonID);

            // Properties
            this.Property(t => t.Comment)
                .IsRequired();

            // Table & Column Mappings
           
            this.Property(t => t.CommentedBy).HasColumnName("CommentedBy");
            this.Property(t => t.CommentedDate).HasColumnName("CommentedDate");
            this.Property(t => t.EditedDate).HasColumnName("EditedDate");
            this.Property(t => t.Attachment).HasColumnName("Attachment");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.RegionalRequestID).HasColumnName("RegionalRequestID");
            this.Property(t => t.PartitionId).HasColumnName("PartitionId");
            

           

        }
    }
}
