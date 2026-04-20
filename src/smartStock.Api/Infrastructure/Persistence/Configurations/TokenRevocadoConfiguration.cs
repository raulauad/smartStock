using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Configurations;

public sealed class TokenRevocadoConfiguration : IEntityTypeConfiguration<TokenRevocado>
{
    public void Configure(EntityTypeBuilder<TokenRevocado> builder)
    {
        builder.ToTable("TokensRevocados");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Jti)
            .IsRequired()
            .HasMaxLength(36);

        builder.HasIndex(t => t.Jti)
            .IsUnique();

        builder.Property(t => t.Expiracion)
            .IsRequired();

        builder.Property(t => t.UsuarioId)
            .IsRequired();
    }
}
