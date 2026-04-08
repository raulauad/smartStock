namespace smartStock.Api.Domain.Models;

public sealed class Direccion
{
    public string Pais         { get; set; } = string.Empty;
    public string Provincia    { get; set; } = string.Empty;
    public string Localidad    { get; set; } = string.Empty;
    public string CodigoPostal { get; set; } = string.Empty;
    public string Calle        { get; set; } = string.Empty;
    public string Numero       { get; set; } = string.Empty;
}
