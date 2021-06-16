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
            builder.Property(c => c.Username).HasColumnName("Username");
            builder.Property(c => c.Password).HasColumnName("Password");
            builder.Property(c => c.PhoneNumber).HasColumnName("PhoneNumber");

            builder.OwnsOne(c => c.LoginVerification,
                    phone =>
                    {
                        phone.Property(p => p.VerificationCode).HasColumnName("LoginVerificationCode");
                        phone.Property(p => p.ExpiryDateTime).HasColumnName("VerificationExpiryDateTime");
                    });
        }
    }
}