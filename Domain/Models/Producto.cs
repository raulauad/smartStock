namespace smartStock.Domain.Models;

public class Producto
{
    public int     Id          { get; set; }
    public string  Nombre      { get; set; } = string.Empty;
    public string  Descripcion { get; set; } = string.Empty;
    public decimal PrecioCosto { get; set; }   // decimal(12,2)
    public decimal PrecioVenta { get; set; }   // decimal(12,2)

    public int       CategoriaId   { get; set; }
    public Categoria Categoria     { get; set; } = null!;
    public Guid      UsuarioAltaId { get; set; }
    public Usuario   UsuarioAlta   { get; set; } = null!;

    public StockActual                  Stock       { get; set; } = null!;
    public ICollection<MovimientoStock> Movimientos { get; set; } = [];
}
