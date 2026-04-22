using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Infrastructure.Persistence;
using smartStock.Shared.Dtos.Admin.ObtenerListaCategorias;

namespace smartStock.Api.Application.Features.Admin.Queries.ObtenerListaCategorias;

public sealed class ObtenerListaCategoriasQueryHandler
    : IRequestHandler<ObtenerListaCategoriasQuery, IReadOnlyList<ObtenerListaCategoriasResponse>>
{
    private readonly AppDbContext _db;

    public ObtenerListaCategoriasQueryHandler(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<ObtenerListaCategoriasResponse>> Handle(
        ObtenerListaCategoriasQuery query,
        CancellationToken           cancellationToken)
    {
        var q = _db.Categorias.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.FiltroEstado))
        {
            q = query.FiltroEstado.ToLowerInvariant() switch
            {
                "activo"   => q.Where(c => c.EstaActivo),
                "inactivo" => q.Where(c => !c.EstaActivo),
                _          => q
            };
        }

        if (!string.IsNullOrWhiteSpace(query.Busqueda))
        {
            var busqueda = query.Busqueda.Trim();
            q = q.Where(c => c.Nombre.Contains(busqueda));
        }

        return await q
            .OrderBy(c => c.Nombre)
            .Select(c => new ObtenerListaCategoriasResponse(
                c.Id,
                c.Nombre,
                c.Descripcion,
                c.EstaActivo,
                c.Productos.Count))
            .ToListAsync(cancellationToken);
    }
}
