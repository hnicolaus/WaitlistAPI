﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelMaps
{
    public class PartyMap : IEntityTypeConfiguration<Party>
    {
        /**
         * specify entity configuration in a separate class
         * instead of in directly in DbContext.OnModelCreating(ModelBuilder modelBuilder), i.e.
         * modelBuilder.Entity<Waitlist>(entity =>
         * {
         *     entity.Property(e => e.Id).HasColumnName("Id");
         *     ...
         * });
         */
        public void Configure(EntityTypeBuilder<Party> builder)
        {
            builder.ToTable(schema: "dbo", name: "Party").HasKey(w => w.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.CreatedDateTime).HasColumnName("CreatedDateTime");
            builder.Property(p => p.PartySize).HasColumnName("PartySize");
            builder.Property(p => p.IsNotified).HasColumnName("IsNotified");
            builder.Property(p => p.IsActive).HasColumnName("IsActive");
            builder.Property(p => p.CustomerId).HasColumnName("CustomerId");

            builder.HasOne(p => p.Customer).WithMany(c => c.Parties).HasForeignKey(p => p.CustomerId);
        }
    }
}