using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Configurations;

public sealed class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.ContrasenaHash)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.Dni)
            .IsRequired()
            .HasMaxLength(8)
            .IsFixedLength();

        builder.HasIndex(u => u.Dni)
            .IsUnique();

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
    }
}
