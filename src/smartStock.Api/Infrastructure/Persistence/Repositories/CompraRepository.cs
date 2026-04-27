using Microsoft.EntityFrameworkCore;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Enums;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Repositories;

public sealed class CompraRepository : ICompraRepository
{
    private readonly AppDbContext _db;

    public CompraRepository(AppDbContext db) => _db = db;

    public Task<CompraDia?> ObtenerSesionDiariaPorFechaAsync(DateTime fecha, CancellationToken ct = default)
        => _db.ComprasDia
              .FirstOrDefaultAsync(c => c.FechaSesion == fecha.Date, ct);

    public Task<bool> ComprobanteExisteAsync(Guid proveedorId, string numero, TipoComprobante tipo, int? excluirCompraId, CancellationToken ct = default)
        => _db.DetallesCompra.AnyAsync(d =>
            !d.EstaAnulada &&
            d.ProveedorId == proveedorId &&
            d.NumeroComprobante == numero &&
            d.TipoComprobante == tipo &&
            (excluirCompraId == null || d.Id != excluirCompraId), ct);

    public async Task<int> RegistrarCompraAsync(CompraDia sesion, bool esNuevaSesion, DetalleCompra compra, CancellationToken ct = default)
    {
        if (esNuevaSesion)
            _db.ComprasDia.Add(sesion);

        _db.DetallesCompra.Add(compra);
        await _db.SaveChangesAsync(ct);
        return compra.Id;
    }

    public Task<DetalleCompra?> ObtenerCompraPorIdConItemsAsync(int compraId, CancellationToken ct = default)
        => _db.DetallesCompra
              .Include(d => d.Items)
                  .ThenInclude(i => i.Movimientos)
              .Include(d => d.CompraDia)
              .FirstOrDefaultAsync(d => d.Id == compraId, ct);

    public Task GuardarCambiosAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
