using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Configurations;

public sealed class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.Property(u => u.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        // DNI: longitud fija de 8 caracteres definida por el sistema
        builder.Property(u => u.Dni)
            .IsRequired()
            .HasMaxLength(8)
            .IsFixedLength();

        builder.Property(u => u.Telefono)
            .IsRequired()
            .HasMaxLength(20);

        builder.OwnsOne(u => u.Direccion, d =>
        {
            d.Property(x => x.Pais)         .IsRequired().HasMaxLength(60);
            d.Property(x => x.Provincia)    .IsRequired().HasMaxLength(60);
            d.Property(x => x.Localidad)    .IsRequired().HasMaxLength(60);
            d.Property(x => x.CodigoPostal) .IsRequired().HasMaxLength(10);
            d.Property(x => x.Calle)        .IsRequired().HasMaxLength(100);
            d.Property(x => x.Numero)       .IsRequired().HasMaxLength(10);
        });

        builder.Property(u => u.FechaAlta)
            .IsRequired();

        builder.Property(u => u.EstaActivo)
            .IsRequired()
            .HasDefaultValue(true);

        // Índice único sobre DNI
        builder.HasIndex(u => u.Dni)
            .IsUnique();
    }
}
