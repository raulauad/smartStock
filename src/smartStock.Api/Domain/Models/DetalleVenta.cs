using smartStock.Api.Domain.Enums;

namespace smartStock.Api.Domain.Models;

public class DetalleVenta
{
    public int       Id                { get; set; }
    public DateTime  FechaHora         { get; set; } = DateTime.UtcNow;
    public decimal   Total             { get; set; }
    public int       NumeroComprobante { get; set; }
    public FormaPago FormaPago         { get; set; }
    public decimal?  MontoRecibido     { get; set; }
    public bool      EstaAnulada       { get; set; }
    public DateTime? FechaAnulacion    { get; set; }
    public string?   MotivoAnulacion   { get; set; }

    public int      VentaDiaId { get; set; }
    public VentaDia VentaDia   { get; set; } = null!;

    public Guid    UsuarioId { get; set; }
    public Usuario Usuario   { get; set; } = null!;

    public Guid?    UsuarioAnulaId { get; set; }
    public Usuario? UsuarioAnula   { get; set; }

    public ICollection<ItemDetalleVenta> Items { get; set; } = [];
}
