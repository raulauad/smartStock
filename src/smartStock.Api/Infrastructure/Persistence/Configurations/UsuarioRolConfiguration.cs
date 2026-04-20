using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Configurations;

public sealed class UsuarioRolConfiguration : IEntityTypeConfiguration<UsuarioRol>
{
    public void Configure(EntityTypeBuilder<UsuarioRol> builder)
    {
        builder.ToTable("UsuarioRoles");

        builder.HasKey(ur => new { ur.UsuarioId, ur.Rol });

        builder.Property(ur => ur.Rol)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasOne(ur => ur.Usuario)
            .WithMany(u => u.Roles)
            .HasForeignKey(ur => ur.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        // Garantiza que solo exista un Administrador en el sistema (protección a nivel BD)
        builder.HasIndex(ur => ur.Rol)
            .IsUnique()
            .HasFilter("[Rol] = 'Administrador'")
            .HasDatabaseName("UX_UsuarioRoles_Administrador");
    }
}
