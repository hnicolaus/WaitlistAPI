using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelMaps
{
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable(schema: "dbo", name: "Customer").HasKey(w => w.Id);

            builder.Property(w => w.Id).ValueGeneratedOnAdd();
            builder.Property(w => w.UserName).HasColumnName(nameof(Customer.UserName));
            builder.Property(w => w.FirstName).HasColumnName(nameof(Customer.FirstName));
            builder.Property(w => w.LastName).HasColumnName(nameof(Customer.LastName));
            builder.Property(w => w.Address).HasColumnName(nameof(Customer.Address));
            builder.Property(w => w.PhoneNumber).HasColumnName(nameof(Customer.PhoneNumber));
            builder.Property(w => w.IsPhoneNumberValidated).HasColumnName(nameof(Customer.IsPhoneNumberValidated));
            builder.Property(w => w.PhoneNumberValidationCode).HasColumnName(nameof(Customer.PhoneNumberValidationCode));
            builder.Property(w => w.DateOfBirth).HasColumnName(nameof(Customer.DateOfBirth));
            builder.Property(w => w.CreatedDateTime).HasColumnName(nameof(Customer.CreatedDateTime));
        }
    }
}