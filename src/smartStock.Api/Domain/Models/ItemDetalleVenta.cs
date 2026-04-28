namespace smartStock.Api.Domain.Models;

public class ItemDetalleVenta
{
    public int      Id            { get; set; }
    public decimal  Cantidad      { get; set; }
    public decimal  PrecioVenta   { get; set; }    // snapshot
    public decimal  PrecioCosto   { get; set; }    // snapshot
    public decimal  Subtotal      { get; set; }    // calculado en app: Cantidad * PrecioVenta
    public decimal  GananciaTotal { get; set; }    // calculado en app: (PrecioVenta - PrecioCosto) * Cantidad
    public string   NombreProducto { get; set; } = string.Empty;  // snapshot
    public decimal? Factor        { get; set; }    // snapshot del factor del código usado

    public int          DetalleVentaId { get; set; }
    public DetalleVenta DetalleVenta   { get; set; } = null!;

    public Guid     ProductoId { get; set; }
    public Producto Producto   { get; set; } = null!;

    public Guid?           CodigoProductoId { get; set; }
    public CodigoProducto? CodigoProducto   { get; set; }

    // 1:many: movimiento original Venta + compensatorio Anulacion (si fue anulada)
    public ICollection<MovimientoStock> Movimientos { get; set; } = [];
}
