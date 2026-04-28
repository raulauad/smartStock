using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Infrastructure.Persistence;
using smartStock.Shared.Dtos.Ventas.ObtenerSesionDiariaVentas;

namespace smartStock.Api.Application.Features.Ventas.Queries.ObtenerSesionDiariaVentas;

public sealed class ObtenerSesionDiariaVentasQueryHandler
    : IRequestHandler<ObtenerSesionDiariaVentasQuery, ObtenerSesionDiariaVentasResponse?>
{
    private readonly AppDbContext _db;

    public ObtenerSesionDiariaVentasQueryHandler(AppDbContext db) => _db = db;

    public async Task<ObtenerSesionDiariaVentasResponse?> Handle(ObtenerSesionDiariaVentasQuery query, CancellationToken ct)
    {
        var fechaBusqueda = (query.Fecha ?? DateTime.UtcNow).Date;

        var sesion = await _db.VentasDia
            .AsNoTracking()
            .Include(v => v.Usuario)
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Usuario)
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Items)
            .FirstOrDefaultAsync(v => v.FechaSesion == fechaBusqueda, ct);

        if (sesion is null)
            return null;

        var ventasVigentes = sesion.Detalles.Where(d => !d.EstaAnulada).ToList();

        var totalPorFormaPago = ventasVigentes
            .GroupBy(d => d.FormaPago.ToString())
            .ToDictionary(g => g.Key, g => g.Sum(d => d.Total));

        decimal? gananciaBruta = query.EsAdmin
            ? ventasVigentes.SelectMany(d => d.Items).Sum(i => i.GananciaTotal)
            : null;

        var ventasResumen = sesion.Detalles
            .OrderBy(d => d.NumeroComprobante)
            .Select(d => new ResumenVentaResponse(
                d.Id,
                d.NumeroComprobante,
                d.FechaHora,
                d.Usuario.Nombre,
                d.Total,
                d.Items.Count,
                d.FormaPago.ToString(),
                d.EstaAnulada
            ))
            .ToList();

        return new ObtenerSesionDiariaVentasResponse(
            sesion.Id,
            sesion.FechaSesion,
            sesion.Estado.ToString(),
            sesion.Usuario.Nombre,
            ventasVigentes.Sum(d => d.Total),
            ventasVigentes.Count,
            sesion.Detalles.Count(d => d.EstaAnulada),
            totalPorFormaPago,
            gananciaBruta,
            ventasResumen
        );
    }
}
