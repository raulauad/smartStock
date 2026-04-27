namespace smartStock.Api.Domain.Models;

public class ItemDetalleCompra
{
    public int     Id            { get; set; }
    public decimal Cantidad      { get; set; }
    public decimal PrecioCompra  { get; set; }   // snapshot
    public decimal Subtotal      { get; set; }   // calculado en app: Cantidad * PrecioCompra
    public string  NombreProducto { get; set; } = string.Empty;  // snapshot
    public decimal? Factor       { get; set; }   // snapshot del Factor del código usado (null si se eligió por listado)

    public int           DetalleCompraId { get; set; }
    public DetalleCompra DetalleCompra   { get; set; } = null!;

    public Guid     ProductoId { get; set; }
    public Producto Producto   { get; set; } = null!;

    public Guid?           CodigoProductoId { get; set; }
    public CodigoProducto? CodigoProducto   { get; set; }

    public ICollection<MovimientoStock> Movimientos { get; set; } = [];
}
