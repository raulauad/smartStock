using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartStock.Domain.Models;

namespace smartStock.Infrastructure.Persistence.Configurations;

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
            .HasMaxLength(300);

        // Nombre de categoría único en el sistema
        builder.HasIndex(c => c.Nombre)
            .IsUnique();
    }
}
