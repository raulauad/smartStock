using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Configurations;

public sealed class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Nombre)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Descripcion)
            .HasMaxLength(250);

        builder.Property(c => c.EstaActivo)
            .IsRequired();

        builder.Property(c => c.FechaAlta)
            .IsRequired();

        builder.HasOne(c => c.UsuarioAlta)
            .WithMany()
            .HasForeignKey(c => c.UsuarioAltaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.Nombre)
            .IsUnique();
    }
}
