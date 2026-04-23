using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Application.Common.Exceptions.Productos;
using smartStock.Api.Infrastructure.Persistence;
using smartStock.Shared.Dtos.Admin.ObtenerDetalleProducto;
using smartStock.Shared.Dtos.Admin.Productos;

namespace smartStock.Api.Application.Features.Admin.Queries.ObtenerDetalleProducto;

public sealed class ObtenerDetalleProductoQueryHandler
    : IRequestHandler<ObtenerDetalleProductoQuery, ObtenerDetalleProductoResponse>
{
    private readonly AppDbContext _db;

    public ObtenerDetalleProductoQueryHandler(AppDbContext db) => _db = db;

    public async Task<ObtenerDetalleProductoResponse> Handle(
        ObtenerDetalleProductoQuery query,
        CancellationToken           cancellationToken)
    {
        var producto = await _db.Productos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .Include(p => p.Stock)
            .Include(p => p.Codigos)
            .Include(p => p.UsuarioAlta)
            .FirstOrDefaultAsync(p => p.Id == query.ProductoId, cancellationToken)
            ?? throw new ProductoNoEncontradoException();

        return new ObtenerDetalleProductoResponse(
            producto.Id,
            producto.Nombre,
            producto.Descripcion,
            producto.Categoria.Nombre,
            producto.UnidadMedida.ToString(),
            producto.PrecioCosto,
            producto.PrecioVenta,
            producto.Stock.Cantidad,
            producto.StockMinimo,
            producto.EstaActivo,
            producto.FechaAlta,
            producto.UsuarioAlta.Nombre,
            producto.Codigos
                .Select(c => new CodigoProductoResponse(c.Id, c.Codigo, c.Tipo.ToString(), c.Factor, c.Descripcion))
                .ToList()
        );
    }
}
