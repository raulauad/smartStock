namespace smartStock.Api.Domain.Models;

public class ItemDetalleCompra
{
    public int     Id           { get; set; }
    public decimal Cantidad     { get; set; }
    public decimal PrecioCompra { get; set; }   // snapshot
    public decimal Subtotal     { get; set; }   // calculado en app: Cantidad * PrecioCompra

    public int           DetalleCompraId { get; set; }
    public DetalleCompra DetalleCompra   { get; set; } = null!;

    public Guid     ProductoId { get; set; }
    public Producto Producto   { get; set; } = null!;

    public MovimientoStock? Movimiento { get; set; }
}
