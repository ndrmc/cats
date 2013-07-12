﻿using System.Data.Entity.ModelConfiguration;

namespace Cats.Models.Mapping
{
    public class RegionalRequestDetailMap:EntityTypeConfiguration<RegionalRequestDetail>
    {
        public RegionalRequestDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.RegionalRequestDetailID);

            // Properties
            this.Property(t => t.Beneficiaries)
                .IsRequired();



            // Table & Column Mappings
            this.ToTable("EarlyWarning.RegionalRequestDetail");
            this.Property(t => t.RegionalRequestDetailID).HasColumnName("RegionalRequestDetailID");
            this.Property(t => t.RegionalRequestID).HasColumnName("RegionalRequestID");
            this.Property(t => t.Fdpid).HasColumnName("FDPID");
            this.Property(t => t.Beneficiaries).HasColumnName("Beneficiaries");
            this.Property(t => t.Grain).HasColumnName("Grain");
            this.Property(t => t.Pulse).HasColumnName("Pulse");
            this.Property(t => t.Oil).HasColumnName("Oil");
            this.Property(t => t.CSB).HasColumnName("CSB");
            

            // Relationships
            this.HasRequired(t => t.RegionalRequest)
                .WithMany(t => t.RegionalRequestDetails)
                .HasForeignKey(d => d.RegionalRequestID);

            this.HasRequired(t => t.Fdp)
                .WithMany(t => t.RegionalRequestDetails)
                .HasForeignKey(d => d.Fdpid);

        }
    }
}