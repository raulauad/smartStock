using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Configurations;

public sealed class ProveedorConfiguration : IEntityTypeConfiguration<Proveedor>
{
    public void Configure(EntityTypeBuilder<Proveedor> builder)
    {
        builder.ToTable("Proveedores");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Cuit)
            .HasMaxLength(11)
            .IsFixedLength();

        builder.HasIndex(p => p.Cuit)
            .IsUnique()
            .HasFilter("[Cuit] IS NOT NULL");

        builder.HasIndex(p => p.Nombre)
            .IsUnique();

        builder.Property(p => p.Telefono)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(p => p.Telefono)
            .IsUnique();

        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(p => p.Email)
            .IsUnique();

        builder.OwnsOne(p => p.Direccion, d =>
        {
            d.Property(x => x.Pais)        .IsRequired().HasMaxLength(60);
            d.Property(x => x.Provincia)   .IsRequired().HasMaxLength(60);
            d.Property(x => x.Localidad)   .IsRequired().HasMaxLength(60);
            d.Property(x => x.CodigoPostal).IsRequired().HasMaxLength(10);
            d.Property(x => x.Calle)       .IsRequired().HasMaxLength(100);
            d.Property(x => x.Numero)      .IsRequired().HasMaxLength(10);
        });

        builder.Property(p => p.Observaciones)
            .HasMaxLength(500);

        builder.Property(p => p.FechaAlta)
            .IsRequired();

        builder.Property(p => p.EstaActivo)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasOne(p => p.UsuarioAlta)
            .WithMany()
            .HasForeignKey(p => p.UsuarioAltaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
