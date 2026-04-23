using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Configurations;

public sealed class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Descripcion)
            .HasMaxLength(500);

        builder.Property(p => p.PrecioCosto)
            .HasColumnType("decimal(12,2)")
            .IsRequired();

        builder.Property(p => p.PrecioVenta)
            .HasColumnType("decimal(12,2)")
            .IsRequired();

        builder.Property(p => p.StockMinimo)
            .HasColumnType("decimal(12,2)")
            .IsRequired();

        builder.Property(p => p.UnidadMedida)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.EstaActivo)
            .IsRequired();

        builder.Property(p => p.FechaAlta)
            .IsRequired();
    }
}
