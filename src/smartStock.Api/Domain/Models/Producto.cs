using smartStock.Api.Domain.Enums;

namespace smartStock.Api.Domain.Models;

public class Producto
{
    public Guid         Id           { get; set; }
    public string       Nombre       { get; set; } = string.Empty;
    public string?      Descripcion  { get; set; }
    public decimal      PrecioCosto  { get; set; }
    public decimal      PrecioVenta  { get; set; }
    public UnidadMedida UnidadMedida { get; set; }
    public decimal      StockMinimo  { get; set; }
    public bool         EstaActivo   { get; set; }
    public DateTime     FechaAlta    { get; set; }

    public Guid      CategoriaId   { get; set; }
    public Categoria Categoria     { get; set; } = null!;
    public Guid      UsuarioAltaId { get; set; }
    public Usuario   UsuarioAlta   { get; set; } = null!;

    public StockActual                  Stock       { get; set; } = null!;
    public ICollection<CodigoProducto>  Codigos     { get; set; } = [];
    public ICollection<MovimientoStock> Movimientos { get; set; } = [];
}
