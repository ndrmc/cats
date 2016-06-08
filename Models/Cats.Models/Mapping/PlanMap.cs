﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models.Mapping
{
   public class PlanMap:EntityTypeConfiguration<Plan>
   {

       public PlanMap()
       {
           // Primary Key
           this.HasKey(t => t.PlanID);

            this.ToTable("Plan","dbo");
            this.Property(t => t.PlanID).HasColumnName("PlanID");
            this.Property(t => t.BusinessProcessID).HasColumnName("BusinessProcessID");
            this.Property(t => t.PlanName).HasColumnName("PlanName");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
           this.Property(t => t.ProgramID).HasColumnName("ProgramID");
           this.Property(t => t.Remark).HasColumnName("Remark");
           this.Property(t => t.Status).HasColumnName("Status");
           this.Property(t => t.Duration).HasColumnName("Duration");
           this.Property(t => t.PartitionId).HasColumnName("PartitionId");
            //this.HasRequired(t => t.Program)
            //   .WithMany(t => t.Plans)
            //   .HasForeignKey(d => d.ProgramID);
            this.HasRequired(t => t.BusinessProcess)
                 .WithMany(t => t.Plans)
                 .HasForeignKey(d => d.BusinessProcessID);
        }
       

    }
}
