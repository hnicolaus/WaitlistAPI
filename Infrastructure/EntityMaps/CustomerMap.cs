using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelMaps
{
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable(schema: "dbo", name: "Customer").HasKey(w => w.Id);

            builder.Property(c => c.Id);
            builder.Property(c => c.FirstName).HasColumnName("FirstName");
            builder.Property(c => c.LastName).HasColumnName("LastName");
            builder.Property(c => c.Email).HasColumnName("Email");
            builder.Property(c => c.CreatedDateTime).HasColumnName("CreatedDateTime");

            builder.OwnsOne(c => c.Phone,
                    phone =>
                    {
                        phone.Property(p => p.PhoneNumber).HasColumnName("PhoneNumber");
                        phone.Property(p => p.IsVerified).HasColumnName("IsPhoneNumberVerified");
                        phone.OwnsOne(v => v.Verification,
                            verification =>
                            {
                                verification.Property(v => v.VerificationCode).HasColumnName("PhoneNumberVerificationCode");
                                verification.Property(v => v.ExpiryDateTime).HasColumnName("VerificationExpiryDateTime");
                            });
                    });
        }
    }
}