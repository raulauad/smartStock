using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Infrastructure.Persistence;
using smartStock.Shared.Dtos.Compras.ObtenerSesionDiariaCompras;

namespace smartStock.Api.Application.Features.Compras.Queries.ObtenerSesionDiariaCompras;

public sealed class ObtenerSesionDiariaComprasQueryHandler
    : IRequestHandler<ObtenerSesionDiariaComprasQuery, ObtenerSesionDiariaComprasResponse?>
{
    private readonly AppDbContext _db;

    public ObtenerSesionDiariaComprasQueryHandler(AppDbContext db) => _db = db;

    public async Task<ObtenerSesionDiariaComprasResponse?> Handle(ObtenerSesionDiariaComprasQuery query, CancellationToken ct)
    {
        var fechaBusqueda = (query.Fecha ?? DateTime.UtcNow).Date;

        var sesion = await _db.ComprasDia
            .AsNoTracking()
            .Include(c => c.Usuario)
            .Include(c => c.Detalles)
                .ThenInclude(d => d.Proveedor)
            .Include(c => c.Detalles)
                .ThenInclude(d => d.Items)
            .FirstOrDefaultAsync(c => c.FechaSesion == fechaBusqueda, ct);

        if (sesion is null)
            return null;

        var comprasResumen = sesion.Detalles
            .Select(d => new ResumenCompraResponse(
                d.Id,
                d.Proveedor.Nombre,
                d.FechaCompra,
                d.Total,
                d.Items.Count,
                d.EstaAnulada,
                d.NumeroComprobante,
                d.TipoComprobante?.ToString()
            ))
            .ToList();

        return new ObtenerSesionDiariaComprasResponse(
            sesion.Id,
            sesion.FechaSesion,
            sesion.Estado.ToString(),
            sesion.Usuario.Nombre,
            sesion.Detalles.Where(d => !d.EstaAnulada).Sum(d => d.Total),
            sesion.Detalles.Count(d => !d.EstaAnulada),
            sesion.Detalles.Count(d => d.EstaAnulada),
            comprasResumen
        );
    }
}
