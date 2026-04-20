namespace smartStock.Api.Domain.Models;

public class Usuario
{
    public Guid      Id             { get; set; }
    public string    Nombre         { get; set; } = string.Empty;
    public string    Email          { get; set; } = string.Empty;
    public string    ContrasenaHash { get; set; } = string.Empty;
    public string    Dni            { get; set; } = string.Empty;
    public string    Telefono       { get; set; } = string.Empty;
    public Direccion Direccion      { get; set; } = new();
    public DateTime  FechaAlta      { get; set; } = DateTime.UtcNow;
    public DateTime? FechaBaja      { get; set; }
    public bool      EstaActivo     { get; set; } = true;

    public ICollection<UsuarioRol>       Roles          { get; set; } = [];
    public ICollection<Producto>         Productos       { get; set; } = [];
    public ICollection<MovimientoStock>  Movimientos     { get; set; } = [];
    public ICollection<VentaDia>  VentasDia  { get; set; } = [];
    public ICollection<CompraDia> ComprasDia { get; set; } = [];
}
