using smartStock.Api.Domain.Enums;
namespace smartStock.Api.Domain.Models;

public class DetalleCompra
{
    public int      Id        { get; set; }
    public DateTime FechaHora { get; set; } = DateTime.UtcNow;
    public DateTime FechaCompra { get; set; }
    public decimal  Total     { get; set; }

    // Comprobante (todos opcionales, unicidad en Proveedor+Numero+Tipo)
    public string?          NumeroComprobante { get; set; }
    public TipoComprobante? TipoComprobante   { get; set; }
    public DateTime?        FechaComprobante  { get; set; }

    // Anulación
    public bool      EstaAnulada     { get; set; }
    public DateTime? FechaAnulacion  { get; set; }
    public string?   MotivoAnulacion { get; set; }
    public Guid?     UsuarioAnulaId  { get; set; }
    public Usuario?  UsuarioAnula    { get; set; }

    public int       CompraDiaId { get; set; }
    public CompraDia CompraDia   { get; set; } = null!;

    public Guid      ProveedorId { get; set; }
    public Proveedor Proveedor   { get; set; } = null!;

    public Guid    UsuarioId { get; set; }
    public Usuario Usuario   { get; set; } = null!;

    public ICollection<ItemDetalleCompra> Items { get; set; } = [];
}
