using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Configurations;

public sealed class DetalleCompraConfiguration : IEntityTypeConfiguration<DetalleCompra>
{
    public void Configure(EntityTypeBuilder<DetalleCompra> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.FechaHora)
            .IsRequired();

        builder.Property(d => d.FechaCompra)
            .IsRequired();

        builder.Property(d => d.Total)
            .HasColumnType("decimal(12,2)")
            .IsRequired();

        builder.Property(d => d.NumeroComprobante)
            .HasMaxLength(30);

        builder.Property(d => d.TipoComprobante)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(d => d.MotivoAnulacion)
            .HasMaxLength(500);

        // Índice único: un proveedor no puede tener dos compras con el mismo número y tipo de comprobante (no anuladas)
        builder.HasIndex(d => new { d.ProveedorId, d.NumeroComprobante, d.TipoComprobante })
            .IsUnique()
            .HasFilter("[NumeroComprobante] IS NOT NULL AND [EstaAnulada] = 0")
            .HasDatabaseName("UX_DetallesCompra_Comprobante");

        // NoAction: Usuario → CompraDia → DetalleCompra ya tiene CASCADE; FK directo no puede tenerlo también
        builder.HasOne(d => d.Usuario)
            .WithMany()
            .HasForeignKey(d => d.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(d => d.UsuarioAnula)
            .WithMany()
            .HasForeignKey(d => d.UsuarioAnulaId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(d => d.Proveedor)
            .WithMany(p => p.Detalles)
            .HasForeignKey(d => d.ProveedorId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
