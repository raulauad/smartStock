using smartStock.Api.Domain.Enums;

namespace smartStock.Api.Domain.Models;

public class CodigoProducto
{
    public Guid       Id          { get; set; }
    public string     Codigo      { get; set; } = string.Empty;
    public TipoCodigo Tipo        { get; set; }
    public decimal    Factor      { get; set; }
    public string?    Descripcion { get; set; }

    public Guid     ProductoId { get; set; }
    public Producto Producto   { get; set; } = null!;
}
