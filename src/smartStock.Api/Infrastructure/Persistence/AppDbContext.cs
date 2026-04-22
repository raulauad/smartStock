using Microsoft.EntityFrameworkCore;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario>       Usuarios        { get; set; }
    public DbSet<UsuarioRol>    UsuarioRoles    { get; set; }
    public DbSet<Categoria>     Categorias      { get; set; }
    public DbSet<Producto>      Productos       { get; set; }
    public DbSet<Proveedor>     Proveedores     { get; set; }
    public DbSet<TokenRevocado> TokensRevocados { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
