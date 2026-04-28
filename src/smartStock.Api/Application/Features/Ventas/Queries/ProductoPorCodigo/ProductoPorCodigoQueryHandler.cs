using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Infrastructure.Persistence;
using smartStock.Shared.Dtos.Ventas.ProductoPorCodigo;

namespace smartStock.Api.Application.Features.Ventas.Queries.ProductoPorCodigo;

public sealed class ProductoPorCodigoQueryHandler
    : IRequestHandler<ProductoPorCodigoQuery, ProductoPorCodigoResponse?>
{
    private readonly AppDbContext _db;

    public ProductoPorCodigoQueryHandler(AppDbContext db) => _db = db;

    public async Task<ProductoPorCodigoResponse?> Handle(ProductoPorCodigoQuery query, CancellationToken ct)
    {
        var resultado = await _db.CodigosProducto
            .AsNoTracking()
            .Include(c => c.Producto)
                .ThenInclude(p => p.Stock)
            .Where(c => c.Codigo == query.Codigo)
            .Select(c => new
            {
                CodigoId       = c.Id,
                c.Tipo,
                c.Factor,
                ProductoId     = c.Producto.Id,
                NombreProducto = c.Producto.Nombre,
                c.Producto.PrecioVenta,
                c.Producto.PrecioCosto,
                c.Producto.UnidadMedida,
                c.Producto.EstaActivo,
                StockActual    = c.Producto.Stock.Cantidad
            })
            .FirstOrDefaultAsync(ct);

        if (resultado is null)
            return null;

        return new ProductoPorCodigoResponse(
            resultado.ProductoId,
            resultado.NombreProducto,
            resultado.PrecioVenta,
            query.EsAdmin ? resultado.PrecioCosto : null,
            resultado.StockActual,
            resultado.UnidadMedida.ToString(),
            resultado.EstaActivo,
            resultado.Factor,
            resultado.CodigoId,
            resultado.Tipo.ToString()
        );
    }
}
