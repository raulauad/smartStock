using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Configurations;

public sealed class VentaDiaConfiguration : IEntityTypeConfiguration<VentaDia>
{
    public void Configure(EntityTypeBuilder<VentaDia> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Total)
            .HasColumnType("decimal(12,2)");
    }
}
