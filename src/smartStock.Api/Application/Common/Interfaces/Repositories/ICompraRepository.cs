using smartStock.Api.Domain.Enums;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Application.Common.Interfaces.Repositories;

public interface ICompraRepository
{
    /// <summary>Busca la sesión diaria exacta para la fecha indicada (fecha UTC sin hora).</summary>
    Task<CompraDia?> ObtenerSesionDiariaPorFechaAsync(DateTime fecha, CancellationToken ct = default);

    /// <summary>Verifica si existe una compra vigente (no anulada) con el mismo proveedor, número y tipo de comprobante.</summary>
    Task<bool> ComprobanteExisteAsync(Guid proveedorId, string numero, TipoComprobante tipo, int? excluirCompraId, CancellationToken ct = default);

    /// <summary>Persiste la sesión diaria (nueva o existente con Total actualizado) y la compra con sus ítems y movimientos.</summary>
    Task<int> RegistrarCompraAsync(CompraDia sesion, bool esNuevaSesion, DetalleCompra compra, CancellationToken ct = default);

    /// <summary>Carga la compra con sus ítems, movimientos y la sesión diaria asociada.</summary>
    Task<DetalleCompra?> ObtenerCompraPorIdConItemsAsync(int compraId, CancellationToken ct = default);

    /// <summary>Persiste todos los cambios tracked en el DbContext (anulación, ajustes de stock, movimientos compensatorios).</summary>
    Task GuardarCambiosAsync(CancellationToken ct = default);
}
