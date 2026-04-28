using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Infrastructure.Persistence;
using smartStock.Shared.Dtos.Admin.ObtenerListaProveedores;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Queries.ObtenerListaProveedores;

public sealed class ObtenerListaProveedoresQueryHandler
    : IRequestHandler<ObtenerListaProveedoresQuery, IReadOnlyList<ObtenerListaProveedoresResponse>>
{
    private readonly AppDbContext _db;

    public ObtenerListaProveedoresQueryHandler(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<ObtenerListaProveedoresResponse>> Handle(
        ObtenerListaProveedoresQuery query,
        CancellationToken            cancellationToken)
    {
        var q = _db.Proveedores.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.FiltroEstado))
        {
            q = query.FiltroEstado.ToLowerInvariant() switch
            {
                "activo"   => q.Where(p => p.EstaActivo),
                "inactivo" => q.Where(p => !p.EstaActivo),
                _          => q
            };
        }

        if (!string.IsNullOrWhiteSpace(query.Busqueda))
        {
            var busqueda = query.Busqueda.Trim();
            q = q.Where(p =>
                p.Nombre.Contains(busqueda) ||
                (p.Cuit != null && p.Cuit.Contains(busqueda)) ||
                p.Email.Contains(busqueda));
        }

        return await q
            .OrderBy(p => p.Nombre)
            .Select(p => new ObtenerListaProveedoresResponse(
                p.Id,
                p.Nombre,
                p.Cuit,
                p.Telefono,
                p.Email,
                p.EstaActivo))
            .ToListAsync(cancellationToken);
    }
}
