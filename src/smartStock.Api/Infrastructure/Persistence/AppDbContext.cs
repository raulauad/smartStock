using Microsoft.EntityFrameworkCore;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario>       Usuarios        { get; set; }
    public DbSet<UsuarioRol>    UsuarioRoles    { get; set; }
    public DbSet<Categoria>     Categorias      { get; set; }
    public DbSet<Producto>        Productos        { get; set; }
    public DbSet<CodigoProducto>  CodigosProducto  { get; set; }
    public DbSet<StockActual>     StocksActuales   { get; set; }
    public DbSet<MovimientoStock> MovimientosStock { get; set; }
    public DbSet<Proveedor>       Proveedores      { get; set; }
    public DbSet<TokenRevocado>   TokensRevocados  { get; set; }

    public DbSet<CompraDia>          ComprasDia        { get; set; }
    public DbSet<DetalleCompra>      DetallesCompra    { get; set; }
    public DbSet<ItemDetalleCompra>  ItemsDetalleCompra { get; set; }
    public DbSet<VentaDia>           VentasDia         { get; set; }
    public DbSet<DetalleVenta>       DetallesVenta     { get; set; }
    public DbSet<ItemDetalleVenta>   ItemsDetalleVenta { get; set; }
    public DbSet<CierreCaja>         CierresCaja       { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
