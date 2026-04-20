using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Configurations;

public sealed class CierreCajaConfiguration : IEntityTypeConfiguration<CierreCaja>
{
    public void Configure(EntityTypeBuilder<CierreCaja> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.TotalVentas)
            .HasColumnType("decimal(12,2)");

        builder.Property(c => c.TotalCompras)
            .HasColumnType("decimal(12,2)");

        builder.Property(c => c.SaldoFinal)
            .HasColumnType("decimal(12,2)");

        builder.HasOne(c => c.VentaDia)
            .WithOne(v => v.CierreCaja)
            .HasForeignKey<CierreCaja>(c => c.VentaDiaId);

        builder.HasOne(c => c.CompraDia)
            .WithOne(cd => cd.CierreCaja)
            .HasForeignKey<CierreCaja>(c => c.CompraDiaId);
    }
}
