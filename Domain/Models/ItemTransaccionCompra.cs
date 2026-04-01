namespace smartStock.Domain.Models;

public class ItemTransaccionCompra
{
    public int     Id           { get; set; }
    public decimal Cantidad     { get; set; }
    public decimal PrecioCompra { get; set; }   // snapshot
    public decimal Subtotal     { get; set; }

    public int               TransaccionId { get; set; }
    public TransaccionCompra Transaccion   { get; set; } = null!;

    public int      ProductoId { get; set; }
    public Producto Producto   { get; set; } = null!;

    public MovimientoStock? Movimiento { get; set; }
}
