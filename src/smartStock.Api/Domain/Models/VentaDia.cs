using smartStock.Api.Domain.Enums;

namespace smartStock.Api.Domain.Models;

public class VentaDia
{
    public int          Id          { get; set; }
    public DateTime     FechaSesion { get; set; } = DateTime.UtcNow;
    public decimal      Total       { get; set; }
    public EstadoCierre Estado      { get; set; } = EstadoCierre.Abierto;

    public Guid    UsuarioId { get; set; }
    public Usuario Usuario   { get; set; } = null!;

    public ICollection<DetalleVenta> Detalles   { get; set; } = [];
    public CierreCaja?               CierreCaja { get; set; }
}
