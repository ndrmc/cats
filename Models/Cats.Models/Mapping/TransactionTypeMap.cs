using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cats.Models;
using System.Data.Entity.ModelConfiguration;

namespace Cats.Models.Mapping
{
    public class TransactionTypeMap : EntityTypeConfiguration<TransactionType>
    {
        public TransactionTypeMap()
        {
            this.HasKey(t => t.TransactionTypeID);

            this.ToTable("TransactionType");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t=>t.TransactionTypeID).HasColumnName("TransactionTypeID"); 
            this.Property(t=>t.Description).HasColumnName("Description");
        }


    }
		}
	
