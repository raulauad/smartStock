using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Domain.Enums;
using smartStock.Api.Infrastructure.Persistence;
using smartStock.Shared.Dtos.Ventas.ObtenerListaVentas;

namespace smartStock.Api.Application.Features.Ventas.Queries.ObtenerListaVentas;

public sealed class ObtenerListaVentasQueryHandler
    : IRequestHandler<ObtenerListaVentasQuery, List<ObtenerListaVentasResponse>>
{
    private readonly AppDbContext _db;

    public ObtenerListaVentasQueryHandler(AppDbContext db) => _db = db;

    public async Task<List<ObtenerListaVentasResponse>> Handle(ObtenerListaVentasQuery query, CancellationToken ct)
    {
        var q = _db.DetallesVenta
            .AsNoTracking()
            .Include(d => d.Usuario)
            .Include(d => d.Items)
            .AsQueryable();

        if (query.FechaDesde.HasValue)
            q = q.Where(d => d.FechaHora >= query.FechaDesde.Value.Date);

        if (query.FechaHasta.HasValue)
            q = q.Where(d => d.FechaHora < query.FechaHasta.Value.Date.AddDays(1));

        if (!string.IsNullOrWhiteSpace(query.FiltroFormaPago) &&
            Enum.TryParse<FormaPago>(query.FiltroFormaPago, ignoreCase: true, out var formaPago))
            q = q.Where(d => d.FormaPago == formaPago);

        if (query.UsuarioRegistroId.HasValue)
            q = q.Where(d => d.UsuarioId == query.UsuarioRegistroId.Value);

        if (query.FiltroEstado == "vigente")
            q = q.Where(d => !d.EstaAnulada);
        else if (query.FiltroEstado == "anulada")
            q = q.Where(d => d.EstaAnulada);

        if (query.NumeroComprobante.HasValue)
            q = q.Where(d => d.NumeroComprobante == query.NumeroComprobante.Value);

        var esAdmin = query.EsAdmin;

        return await q
            .OrderByDescending(d => d.FechaHora)
            .Select(d => new ObtenerListaVentasResponse(
                d.Id,
                d.VentaDiaId,
                d.NumeroComprobante,
                d.FechaHora,
                d.Usuario.Nombre,
                d.Total,
                d.Items.Count,
                d.FormaPago.ToString(),
                d.EstaAnulada,
                esAdmin ? (decimal?)d.Items.Sum(i => i.GananciaTotal) : null
            ))
            .ToListAsync(ct);
    }
}
