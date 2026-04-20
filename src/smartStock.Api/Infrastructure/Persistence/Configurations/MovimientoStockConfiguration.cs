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

        // NoAction: Usuarios → Producto → MovimientoStock ya es CASCADE; este FK directo no puede serlo también
        builder.HasOne(m => m.Usuario)
            .WithMany(u => u.Movimientos)
            .HasForeignKey(m => m.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
