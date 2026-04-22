using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Application.Common.Exceptions.Categorias;
using smartStock.Api.Infrastructure.Persistence;
using smartStock.Shared.Dtos.Admin.ObtenerDetalleCategoria;

namespace smartStock.Api.Application.Features.Admin.Queries.ObtenerDetalleCategoria;

public sealed class ObtenerDetalleCategoriaQueryHandler
    : IRequestHandler<ObtenerDetalleCategoriaQuery, ObtenerDetalleCategoriaResponse>
{
    private readonly AppDbContext _db;

    public ObtenerDetalleCategoriaQueryHandler(AppDbContext db) => _db = db;

    public async Task<ObtenerDetalleCategoriaResponse> Handle(
        ObtenerDetalleCategoriaQuery query,
        CancellationToken            cancellationToken)
    {
        return await _db.Categorias
            .Where(c => c.Id == query.CategoriaId)
            .Select(c => new ObtenerDetalleCategoriaResponse(
                c.Id,
                c.Nombre,
                c.Descripcion,
                c.EstaActivo,
                c.FechaAlta,
                c.UsuarioAlta.Nombre,
                c.Productos.Count))
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new CategoriaNoEncontradaException();
    }
}
