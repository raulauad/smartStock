using Microsoft.AspNetCore.Identity;

namespace smartStock.Domain.Models;

public class Usuario : IdentityUser<Guid>
{
    public string   Nombre     { get; set; } = string.Empty;
    public string   Dni        { get; set; } = string.Empty;
    public string   Telefono   { get; set; } = string.Empty;
    public Direccion Direccion  { get; set; } = new();
    public DateTime FechaAlta  { get; set; } = DateTime.UtcNow;
    public bool     EstaActivo { get; set; } = true;

    public ICollection<Producto>       Productos      { get; set; } = [];
    public ICollection<MovimientoStock> Movimientos   { get; set; } = [];
    public ICollection<SesionVenta>    SesionesVenta  { get; set; } = [];
    public ICollection<SesionCompra>   SesionesCompra { get; set; } = [];
}
