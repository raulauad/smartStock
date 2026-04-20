namespace smartStock.Api.Domain.Models;

public class Proveedor
{
    public int    Id        { get; set; }
    public string Nombre    { get; set; } = string.Empty;
    public string Telefono  { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;

    public ICollection<SesionCompra> SesionesCompra { get; set; } = [];
}
