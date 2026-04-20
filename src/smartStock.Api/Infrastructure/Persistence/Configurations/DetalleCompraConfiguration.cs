using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Configurations;

public sealed class DetalleCompraConfiguration : IEntityTypeConfiguration<DetalleCompra>
{
    public void Configure(EntityTypeBuilder<DetalleCompra> builder)
    {
        builder.HasKey(d => d.Id);

        // NoAction: Usuarios → CompraDia → DetalleCompra ya es CASCADE; este FK directo no puede serlo también
        builder.HasOne(d => d.Usuario)
            .WithMany()
            .HasForeignKey(d => d.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
