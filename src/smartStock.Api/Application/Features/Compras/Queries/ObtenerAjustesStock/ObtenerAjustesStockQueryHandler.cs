using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Domain.Enums;
using smartStock.Api.Infrastructure.Persistence;
using smartStock.Shared.Dtos.Compras.ObtenerAjustesStock;

namespace smartStock.Api.Application.Features.Compras.Queries.ObtenerAjustesStock;

public sealed class ObtenerAjustesStockQueryHandler
    : IRequestHandler<ObtenerAjustesStockQuery, List<ObtenerAjustesStockResponse>>
{
    private readonly AppDbContext _db;

    public ObtenerAjustesStockQueryHandler(AppDbContext db) => _db = db;

    public async Task<List<ObtenerAjustesStockResponse>> Handle(ObtenerAjustesStockQuery query, CancellationToken ct)
    {
        var q = _db.MovimientosStock
            .AsNoTracking()
            .Include(m => m.Producto)
            .Include(m => m.Usuario)
            .Where(m => m.Tipo == TipoMovimiento.Ajuste || m.Tipo == TipoMovimiento.Anulacion)
            .AsQueryable();

        if (query.FiltroTipo == "ajuste")
            q = q.Where(m => m.Tipo == TipoMovimiento.Ajuste);
        else if (query.FiltroTipo == "anulacion")
            q = q.Where(m => m.Tipo == TipoMovimiento.Anulacion);

        if (query.FechaDesde.HasValue)
            q = q.Where(m => m.FechaHora >= query.FechaDesde.Value);

        if (query.FechaHasta.HasValue)
            q = q.Where(m => m.FechaHora <= query.FechaHasta.Value.Date.AddDays(1).AddTicks(-1));

        if (query.ProductoId.HasValue)
            q = q.Where(m => m.ProductoId == query.ProductoId.Value);

        if (query.UsuarioId.HasValue)
            q = q.Where(m => m.UsuarioId == query.UsuarioId.Value);

        return await q
            .OrderByDescending(m => m.FechaHora)
            .Select(m => new ObtenerAjustesStockResponse(
                m.Id,
                m.ProductoId,
                m.Producto.Nombre,
                m.Tipo.ToString(),
                m.Cantidad,
                m.Observacion,
                m.Usuario.Nombre,
                m.FechaHora
            ))
            .ToListAsync(ct);
    }
}
