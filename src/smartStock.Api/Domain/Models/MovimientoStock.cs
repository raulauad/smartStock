using smartStock.Api.Domain.Enums;

namespace smartStock.Api.Domain.Models;

public class MovimientoStock
{
    public int            Id        { get; set; }
    public TipoMovimiento Tipo      { get; set; }
    public decimal        Cantidad  { get; set; }
    public DateTime       FechaHora { get; set; } = DateTime.UtcNow;

    public int      ProductoId { get; set; }
    public Producto Producto   { get; set; } = null!;

    public Guid    UsuarioId { get; set; }
    public Usuario Usuario   { get; set; } = null!;

    // FK nullable según tipo de movimiento
    public int?                  ItemVentaId  { get; set; }
    public ItemTransaccionVenta? ItemVenta    { get; set; }
    public int?                  ItemCompraId { get; set; }
    public ItemTransaccionCompra? ItemCompra  { get; set; }
}
