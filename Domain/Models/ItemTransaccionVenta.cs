namespace smartStock.Domain.Models;

public class ItemTransaccionVenta
{
    public int     Id            { get; set; }
    public decimal Cantidad      { get; set; }
    public decimal PrecioVenta   { get; set; }   // snapshot precio al momento
    public decimal PrecioCosto   { get; set; }   // snapshot para cálculo ganancia
    public decimal Subtotal      { get; set; }   // calculado en app: Cantidad * PrecioVenta
    public decimal GananciaTotal { get; set; }   // calculado en app

    public int              TransaccionId { get; set; }
    public TransaccionVenta Transaccion   { get; set; } = null!;

    public int      ProductoId { get; set; }
    public Producto Producto   { get; set; } = null!;

    public MovimientoStock? Movimiento { get; set; }
}
