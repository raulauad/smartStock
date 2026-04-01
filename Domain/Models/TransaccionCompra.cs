namespace smartStock.Domain.Models;

public class TransaccionCompra
{
    public int      Id        { get; set; }
    public DateTime FechaHora { get; set; } = DateTime.UtcNow;

    public int          SesionCompraId { get; set; }
    public SesionCompra SesionCompra   { get; set; } = null!;

    public Guid    UsuarioId { get; set; }
    public Usuario Usuario   { get; set; } = null!;

    public ICollection<ItemTransaccionCompra> Items { get; set; } = [];
}
