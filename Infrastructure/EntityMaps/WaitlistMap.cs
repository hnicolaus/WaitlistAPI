using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelMaps
{
    public class WaitlistMap : IEntityTypeConfiguration<Waitlist>
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
        public void Configure(EntityTypeBuilder<Waitlist> builder)
        {
            builder.ToTable(schema: "dbo", name: "Waitlist").HasKey(w => w.Id);

            builder.Property(w => w.Id).ValueGeneratedOnAdd();
            builder.Property(w => w.CreatedDateTime).HasColumnName(nameof(Waitlist.CreatedDateTime));
            builder.Property(w => w.PartySize).HasColumnName(nameof(Waitlist.PartySize));
            builder.Property(w => w.IsNotified).HasColumnName(nameof(Waitlist.IsNotified));
            builder.Property(w => w.IsActive).HasColumnName(nameof(Waitlist.IsActive));
            builder.Property(w => w.CustomerId).HasColumnName(nameof(Waitlist.CustomerId));

            builder.HasOne(w => w.Customer).WithMany(c => c.Waitlists).HasForeignKey(w => w.CustomerId);
        }
    }
}