using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Configurations;

public sealed class CodigoProductoConfiguration : IEntityTypeConfiguration<CodigoProducto>
{
    public void Configure(EntityTypeBuilder<CodigoProducto> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Codigo)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Tipo)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.Factor)
            .HasColumnType("decimal(10,4)")
            .IsRequired();

        builder.Property(c => c.Descripcion)
            .HasMaxLength(50);

        builder.HasIndex(c => c.Codigo)
            .IsUnique()
            .HasDatabaseName("UX_CodigosProducto_Codigo");

        builder.HasOne(c => c.Producto)
            .WithMany(p => p.Codigos)
            .HasForeignKey(c => c.ProductoId);
    }
}
