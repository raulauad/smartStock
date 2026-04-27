using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Infrastructure.Persistence;
using smartStock.Shared.Dtos.Compras.ObtenerListaCompras;

namespace smartStock.Api.Application.Features.Compras.Queries.ObtenerListaCompras;

public sealed class ObtenerListaComprasQueryHandler
    : IRequestHandler<ObtenerListaComprasQuery, List<ObtenerListaComprasResponse>>
{
    private readonly AppDbContext _db;

    public ObtenerListaComprasQueryHandler(AppDbContext db) => _db = db;

    public async Task<List<ObtenerListaComprasResponse>> Handle(ObtenerListaComprasQuery query, CancellationToken ct)
    {
        var q = _db.DetallesCompra
            .AsNoTracking()
            .Include(d => d.Proveedor)
            .Include(d => d.Usuario)
            .Include(d => d.Items)
            .AsQueryable();

        if (query.FechaDesde.HasValue)
            q = q.Where(d => d.FechaCompra >= query.FechaDesde.Value.Date);

        if (query.FechaHasta.HasValue)
            q = q.Where(d => d.FechaCompra <= query.FechaHasta.Value.Date);

        if (query.ProveedorId.HasValue)
            q = q.Where(d => d.ProveedorId == query.ProveedorId.Value);

        if (query.UsuarioRegistroId.HasValue)
            q = q.Where(d => d.UsuarioId == query.UsuarioRegistroId.Value);

        if (query.FiltroEstado == "vigente")
            q = q.Where(d => !d.EstaAnulada);
        else if (query.FiltroEstado == "anulada")
            q = q.Where(d => d.EstaAnulada);

        if (!string.IsNullOrWhiteSpace(query.NumeroComprobante))
            q = q.Where(d => d.NumeroComprobante != null && d.NumeroComprobante.Contains(query.NumeroComprobante));

        return await q
            .OrderByDescending(d => d.FechaHora)
            .Select(d => new ObtenerListaComprasResponse(
                d.Id,
                d.CompraDiaId,
                d.Proveedor.Nombre,
                d.Usuario.Nombre,
                d.FechaCompra,
                d.FechaHora,
                d.Total,
                d.Items.Count,
                d.EstaAnulada,
                d.NumeroComprobante,
                d.TipoComprobante == null ? null : d.TipoComprobante.ToString()
            ))
            .ToListAsync(ct);
    }
}
