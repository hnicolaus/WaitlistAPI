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
            builder.Property(c => c.FirstName).HasColumnName(nameof(Customer.FirstName));
            builder.Property(c => c.LastName).HasColumnName(nameof(Customer.LastName));
            builder.Property(c => c.Email).HasColumnName(nameof(Customer.Email));
            builder.Property(c => c.CreatedDateTime).HasColumnName(nameof(Customer.CreatedDateTime));

            builder.OwnsOne(c => c.Phone,
                    phone =>
                    {
                        phone.Property(c => c.PhoneNumber).HasColumnName("PhoneNumber");
                        phone.Property(p => p.IsValidated).HasColumnName("IsPhoneNumberValidated");
                        phone.Property(p => p.VerificationCode).HasColumnName("PhoneNumberValidationCode");
                    });
        }
    }
}