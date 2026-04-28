using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Configurations;

public sealed class DetalleVentaConfiguration : IEntityTypeConfiguration<DetalleVenta>
{
    public void Configure(EntityTypeBuilder<DetalleVenta> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.FechaHora)
            .IsRequired();

        builder.Property(d => d.Total)
            .HasColumnType("decimal(12,2)")
            .IsRequired();

        builder.Property(d => d.FormaPago)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(d => d.MontoRecibido)
            .HasColumnType("decimal(12,2)");

        builder.Property(d => d.MotivoAnulacion)
            .HasMaxLength(500);

        // NoAction: Usuario → VentaDia → DetalleVenta ya tiene CASCADE; FK directo no puede tenerlo también
        builder.HasOne(d => d.Usuario)
            .WithMany()
            .HasForeignKey(d => d.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(d => d.UsuarioAnula)
            .WithMany()
            .HasForeignKey(d => d.UsuarioAnulaId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
