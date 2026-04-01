namespace smartStock.Domain.Models;

public class TransaccionVenta
{
    public int      Id           { get; set; }
    public DateTime FechaHora    { get; set; } = DateTime.UtcNow;

    public int         SesionVentaId { get; set; }
    public SesionVenta SesionVenta   { get; set; } = null!;

    public Guid    UsuarioId { get; set; }
    public Usuario Usuario   { get; set; } = null!;

    public ICollection<ItemTransaccionVenta> Items { get; set; } = [];
}
