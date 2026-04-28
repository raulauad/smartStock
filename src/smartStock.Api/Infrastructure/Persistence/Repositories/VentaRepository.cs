using Microsoft.EntityFrameworkCore;
using smartStock.Api.Application.Common.Exceptions.Ventas;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Repositories;

public sealed class VentaRepository : IVentaRepository
{
    private readonly AppDbContext _db;

    public VentaRepository(AppDbContext db) => _db = db;

    public Task<VentaDia?> ObtenerSesionDiariaPorFechaAsync(DateTime fecha, CancellationToken ct = default)
        => _db.VentasDia
              .FirstOrDefaultAsync(v => v.FechaSesion == fecha.Date, ct);

    public async Task<int> RegistrarVentaAsync(
        VentaDia sesion,
        bool esNuevaSesion,
        DetalleVenta venta,
        IReadOnlyList<(Guid ProductoId, string NombreProducto, decimal CantidadEfectiva)> stockUpdates,
        DateTime ahora,
        CancellationToken ct = default)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        try
        {
            // Descuento atómico de stock: UPDATE condicional previene oversell por concurrencia
            var conflictos = new List<string>();
            foreach (var (productoId, nombreProducto, cantidadEfectiva) in stockUpdates)
            {
                var rowsAffected = await _db.Database.ExecuteSqlAsync(
                    $"""
                    UPDATE StocksActuales
                    SET    Cantidad            = Cantidad - {cantidadEfectiva},
                           UltimaActualizacion = {ahora}
                    WHERE  ProductoId = {productoId}
                      AND  Cantidad  >= {cantidadEfectiva}
                    """, ct);

                if (rowsAffected == 0)
                    conflictos.Add(nombreProducto);
            }

            if (conflictos.Count > 0)
                throw new StockInsuficienteParaVentaException(conflictos);

            // NumeroComprobante correlativo por sesión (1 si es nueva, MAX+1 si es existente)
            int numeroComprobante;
            if (esNuevaSesion)
            {
                numeroComprobante = 1;
            }
            else
            {
                var maxNumero = await _db.DetallesVenta
                    .Where(d => d.VentaDiaId == sesion.Id)
                    .Select(d => (int?)d.NumeroComprobante)
                    .MaxAsync(ct) ?? 0;
                numeroComprobante = maxNumero + 1;
            }
            venta.NumeroComprobante = numeroComprobante;

            if (esNuevaSesion)
            {
                sesion.Total += venta.Total;
                _db.VentasDia.Add(sesion);
            }
            else
            {
                // UPDATE atómico para evitar race condition entre ventas concurrentes de la misma sesión
                await _db.Database.ExecuteSqlAsync(
                    $"UPDATE VentasDia SET Total = Total + {venta.Total} WHERE Id = {sesion.Id}", ct);
                _db.Entry(sesion).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }

            _db.DetallesVenta.Add(venta);

            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
            return venta.Id;
        }
        catch
        {
            await tx.RollbackAsync(CancellationToken.None);
            throw;
        }
    }

    public Task<DetalleVenta?> ObtenerVentaPorIdConItemsAsync(int ventaId, CancellationToken ct = default)
        => _db.DetallesVenta
              .Include(d => d.Items)
                  .ThenInclude(i => i.Movimientos)
              .Include(d => d.VentaDia)
              .FirstOrDefaultAsync(d => d.Id == ventaId, ct);

    public Task GuardarCambiosAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
