namespace smartStock.Api.Domain.Models;

public class ItemDetalleVenta
{
    public int     Id            { get; set; }
    public decimal Cantidad      { get; set; }
    public decimal PrecioVenta   { get; set; }   // snapshot precio al momento
    public decimal PrecioCosto   { get; set; }   // snapshot para cálculo ganancia
    public decimal Subtotal      { get; set; }   // calculado en app: Cantidad * PrecioVenta
    public decimal GananciaTotal { get; set; }   // calculado en app: (PrecioVenta - PrecioCosto) * Cantidad

    public int          DetalleVentaId { get; set; }
    public DetalleVenta DetalleVenta   { get; set; } = null!;

    public Guid     ProductoId { get; set; }
    public Producto Producto   { get; set; } = null!;

    public MovimientoStock? Movimiento { get; set; }
}
