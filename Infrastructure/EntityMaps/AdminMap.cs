using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelMaps
{
    public class AdminMap : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.ToTable(schema: "dbo", name: "Admin").HasKey(w => w.Id);

            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Username).HasColumnName(nameof(Admin.Username));
            builder.Property(c => c.Password).HasColumnName(nameof(Admin.Password));
            builder.Property(c => c.PhoneNumber).HasColumnName(nameof(Admin.PhoneNumber));
            builder.Property(c => c.AuthenticationCode).HasColumnName(nameof(Admin.AuthenticationCode));
        }
    }
}