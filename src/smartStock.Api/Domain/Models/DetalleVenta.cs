namespace smartStock.Api.Domain.Models;

public class DetalleVenta
{
    public int      Id        { get; set; }
    public DateTime FechaHora { get; set; } = DateTime.UtcNow;

    public int      VentaDiaId { get; set; }
    public VentaDia VentaDia   { get; set; } = null!;

    public Guid    UsuarioId { get; set; }
    public Usuario Usuario   { get; set; } = null!;

    public ICollection<ItemDetalleVenta> Items { get; set; } = [];
}
