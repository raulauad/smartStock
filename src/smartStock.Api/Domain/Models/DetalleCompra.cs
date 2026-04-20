namespace smartStock.Api.Domain.Models;

public class DetalleCompra
{
    public int      Id        { get; set; }
    public DateTime FechaHora { get; set; } = DateTime.UtcNow;

    public int       CompraDiaId { get; set; }
    public CompraDia CompraDia   { get; set; } = null!;

    public Guid    UsuarioId { get; set; }
    public Usuario Usuario   { get; set; } = null!;

    public ICollection<ItemDetalleCompra> Items { get; set; } = [];
}
