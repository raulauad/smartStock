using smartStock.Api.Domain.Models;

namespace smartStock.Api.Application.Common.Interfaces.Repositories;

public interface IVentaRepository
{
    /// <summary>Busca la sesión diaria exacta para la fecha indicada (fecha UTC sin hora).</summary>
    Task<VentaDia?> ObtenerSesionDiariaPorFechaAsync(DateTime fecha, CancellationToken ct = default);

    /// <summary>
    /// Persiste la venta de forma atómica dentro de una transacción:
    /// descuenta stock vía SQL condicional (previene oversell), calcula el número de comprobante
    /// correlativo, actualiza el total de la sesión y guarda todos los cambios.
    /// Lanza <see cref="Exceptions.Ventas.StockInsuficienteParaVentaException"/> si algún producto no tiene stock suficiente.
    /// </summary>
    Task<int> RegistrarVentaAsync(
        VentaDia sesion,
        bool esNuevaSesion,
        DetalleVenta venta,
        IReadOnlyList<(Guid ProductoId, string NombreProducto, decimal CantidadEfectiva)> stockUpdates,
        DateTime ahora,
        CancellationToken ct = default);

    /// <summary>Carga la venta con sus ítems, movimientos y la sesión diaria asociada.</summary>
    Task<DetalleVenta?> ObtenerVentaPorIdConItemsAsync(int ventaId, CancellationToken ct = default);

    /// <summary>Persiste todos los cambios tracked en el DbContext (anulación, devolución de stock, movimientos compensatorios).</summary>
    Task GuardarCambiosAsync(CancellationToken ct = default);
}
