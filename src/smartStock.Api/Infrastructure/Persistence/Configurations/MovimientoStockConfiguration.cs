using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Configurations;

public sealed class MovimientoStockConfiguration : IEntityTypeConfiguration<MovimientoStock>
{
    public void Configure(EntityTypeBuilder<MovimientoStock> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Cantidad)
            .HasColumnType("decimal(12,2)");

        builder.Property(m => m.Tipo)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(m => m.Observacion)
            .HasMaxLength(500);

        // NoAction: Usuarios → Producto → MovimientoStock ya es CASCADE; este FK directo no puede serlo también
        builder.HasOne(m => m.Usuario)
            .WithMany(u => u.Movimientos)
            .HasForeignKey(m => m.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);

        // ItemVenta: relación 1:0..1 (un ítem de venta tiene a lo sumo un movimiento)
        builder.HasOne(m => m.ItemVenta)
            .WithOne(i => i.Movimiento)
            .HasForeignKey<MovimientoStock>(m => m.ItemVentaId)
            .OnDelete(DeleteBehavior.NoAction);

        // ItemCompra: relación muchos-a-1 (un ítem puede tener movimiento original + compensatorio de anulación)
        // Configurado en ItemDetalleCompraConfiguration con HasMany → WithOne
    }
}
