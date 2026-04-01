using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartStock.Domain.Models;

namespace smartStock.Infrastructure.Persistence.Configurations;

public sealed class StockActualConfiguration : IEntityTypeConfiguration<StockActual>
{
    public void Configure(EntityTypeBuilder<StockActual> builder)
    {
        // ProductoId es PK y FK simultaneamente (shared-PK 1:1)
        builder.HasKey(s => s.ProductoId);

        builder.Property(s => s.Cantidad)
            .HasColumnType("decimal(12,2)");

        builder.Property(s => s.UltimaActualizacion)
            .IsRequired();

        builder.HasOne(s => s.Producto)
            .WithOne(p => p.Stock)
            .HasForeignKey<StockActual>(s => s.ProductoId);
    }
}
