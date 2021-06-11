using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelMaps
{
    public class TableMap : IEntityTypeConfiguration<Table>
    {
        public void Configure(EntityTypeBuilder<Table> builder)
        {
            builder.ToTable(schema: "dbo", name: "Table").HasKey(w => w.Id);

            builder.Property(t => t.Id).ValueGeneratedOnAdd();
            builder.Property(t => t.Number).HasColumnName(nameof(Table.Number));
            builder.Property(t => t.PartySize).HasColumnName(nameof(Table.PartySize));
            builder.Property(t => t.IsAvailable).HasColumnName(nameof(Table.IsAvailable));
        }
    }
}