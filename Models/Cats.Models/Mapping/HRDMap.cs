﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models.Mapping
{
    public class HRDMap : EntityTypeConfiguration<HRD>
    {
        public HRDMap()
        {
            // Primary Key
            this.HasKey(t => t.HRDID);

            // Properties
            // Table & Column Mappings
            this.ToTable("HRD");
            this.Property(t => t.HRDID).HasColumnName("HRDID");
            this.Property(t => t.Year).HasColumnName("Year");
            this.Property(t => t.CreatedBY).HasColumnName("CreatedBy");
            //this.Property(t => t.RevisionNumber).HasColumnName("RevisionNumber");
            this.Property(t => t.CreatedDate).HasColumnName("DateCreated");
            this.Property(t => t.SeasonID).HasColumnName("SeasonID");
            //this.Property(t => t.IsWorkingVersion).HasColumnName("IsWorkingVersion");
            //this.Property(t => t.IsPublished).HasColumnName("IsPublished");
            this.Property(t => t.PublishedDate).HasColumnName("PublishedDate");
            //this.Property(t => t.RationID).HasColumnName("RationID");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.PlanID).HasColumnName("PlanID");
            this.Property(t => t.TransactionGroupID).HasColumnName("TransactionGroupID");
            this.Property(t => t.PartitionId).HasColumnName("PartitionId");
            // Relationships
            //this.HasOptional(t => t.Season)
            //    .WithMany(t => t.Hrds)
            //    .HasForeignKey(d => d.SeasonID);
            this.HasOptional(t => t.UserProfile)
                .WithMany(t => t.Hrds)
                .HasForeignKey(d => d.CreatedBY);

            //this.HasRequired(t => t.Plan)
            //   .WithMany(t => t.Hrds)
            //   .HasForeignKey(d => d.PlanID);
            this.HasRequired(t => t.TransactionGroup)
                .WithMany(t => t.Hrds)
                .HasForeignKey(d => d.TransactionGroupID);
            this.HasOptional(t => t.BusinessProcess).WithMany(t => t.Hrds).HasForeignKey(t => t.BusinessProcessId);
        }
    }
}
