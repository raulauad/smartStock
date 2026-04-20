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

        // NoAction: Usuarios → CompraDia → DetalleCompra → Item ya es CASCADE; Producto → Item no puede serlo también
        builder.HasOne(i => i.Producto)
            .WithMany()
            .HasForeignKey(i => i.ProductoId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
