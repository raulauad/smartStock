using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Configurations;

public sealed class CompraDiaConfiguration : IEntityTypeConfiguration<CompraDia>
{
    public void Configure(EntityTypeBuilder<CompraDia> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.FechaSesion)
            .IsRequired();

        builder.Property(c => c.Total)
            .HasColumnType("decimal(12,2)");

        builder.Property(c => c.Estado)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne(c => c.Usuario)
            .WithMany(u => u.ComprasDia)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
