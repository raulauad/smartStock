using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Infrastructure.Persistence;
using smartStock.Shared.Dtos.Admin.ObtenerListaProductos;

namespace smartStock.Api.Application.Features.Admin.Queries.ObtenerListaProductos;

public sealed class ObtenerListaProductosQueryHandler
    : IRequestHandler<ObtenerListaProductosQuery, List<ObtenerListaProductosResponse>>
{
    private readonly AppDbContext _db;

    public ObtenerListaProductosQueryHandler(AppDbContext db) => _db = db;

    public async Task<List<ObtenerListaProductosResponse>> Handle(
        ObtenerListaProductosQuery query,
        CancellationToken          cancellationToken)
    {
        var q = _db.Productos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .Include(p => p.Stock)
            .Include(p => p.Codigos)
            .AsQueryable();

        if (query.FiltroEstado == "activo")
            q = q.Where(p => p.EstaActivo);
        else if (query.FiltroEstado == "inactivo")
            q = q.Where(p => !p.EstaActivo);

        if (query.FiltroCategoria.HasValue)
            q = q.Where(p => p.CategoriaId == query.FiltroCategoria.Value);

        if (query.AlertaStockBajo == true)
            q = q.Where(p => p.Stock.Cantidad <= p.StockMinimo);

        if (!string.IsNullOrWhiteSpace(query.Busqueda))
        {
            var busqueda = query.Busqueda.Trim();
            q = q.Where(p =>
                p.Nombre.Contains(busqueda) ||
                p.Codigos.Any(c => c.Codigo.Contains(busqueda)));
        }

        return await q
            .Select(p => new ObtenerListaProductosResponse(
                p.Id,
                p.Nombre,
                p.Categoria.Nombre,
                p.UnidadMedida.ToString(),
                p.PrecioVenta,
                p.Stock.Cantidad,
                p.StockMinimo,
                p.EstaActivo,
                p.Stock.Cantidad <= p.StockMinimo
            ))
            .ToListAsync(cancellationToken);
    }
}
