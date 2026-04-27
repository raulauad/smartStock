using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Configurations;

public sealed class ItemDetalleCompraConfiguration : IEntityTypeConfiguration<ItemDetalleCompra>
{
    public void Configure(EntityTypeBuilder<ItemDetalleCompra> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Cantidad)
            .HasColumnType("decimal(12,2)");

        builder.Property(i => i.PrecioCompra)
            .HasColumnType("decimal(12,2)");

        builder.Property(i => i.Subtotal)
            .HasColumnType("decimal(12,2)");

        builder.Property(i => i.Factor)
            .HasColumnType("decimal(12,4)");

        builder.Property(i => i.NombreProducto)
            .IsRequired()
            .HasMaxLength(100);

        // NoAction: Usuarios → CompraDia → DetalleCompra → Item ya es CASCADE; Producto → Item no puede serlo también
        builder.HasOne(i => i.Producto)
            .WithMany()
            .HasForeignKey(i => i.ProductoId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(i => i.CodigoProducto)
            .WithMany()
            .HasForeignKey(i => i.CodigoProductoId)
            .OnDelete(DeleteBehavior.NoAction);

        // Un ítem puede tener múltiples movimientos (original Compra + compensatorio Anulacion)
        builder.HasMany(i => i.Movimientos)
            .WithOne(m => m.ItemCompra)
            .HasForeignKey(m => m.ItemCompraId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
