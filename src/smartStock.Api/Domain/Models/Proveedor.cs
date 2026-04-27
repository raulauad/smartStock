namespace smartStock.Api.Domain.Models;

public class Proveedor
{
    public Guid      Id            { get; set; }
    public string    Nombre        { get; set; } = string.Empty;
    public string?   Cuit          { get; set; }
    public string    Telefono      { get; set; } = string.Empty;
    public string    Email         { get; set; } = string.Empty;
    public Direccion Direccion     { get; set; } = new();
    public string?   Observaciones { get; set; }
    public bool      EstaActivo    { get; set; } = true;
    public DateTime  FechaAlta     { get; set; } = DateTime.UtcNow;

    public Guid    UsuarioAltaId { get; set; }
    public Usuario UsuarioAlta   { get; set; } = null!;

    public ICollection<DetalleCompra> Detalles { get; set; } = [];
}
