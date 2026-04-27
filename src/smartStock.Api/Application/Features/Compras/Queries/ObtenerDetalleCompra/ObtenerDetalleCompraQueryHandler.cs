using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Application.Common.Exceptions.Compras;
using smartStock.Api.Infrastructure.Persistence;
using smartStock.Shared.Dtos.Compras.ObtenerDetalleCompra;

namespace smartStock.Api.Application.Features.Compras.Queries.ObtenerDetalleCompra;

public sealed class ObtenerDetalleCompraQueryHandler
    : IRequestHandler<ObtenerDetalleCompraQuery, ObtenerDetalleCompraResponse>
{
    private readonly AppDbContext _db;

    public ObtenerDetalleCompraQueryHandler(AppDbContext db) => _db = db;

    public async Task<ObtenerDetalleCompraResponse> Handle(ObtenerDetalleCompraQuery query, CancellationToken ct)
    {
        var compra = await _db.DetallesCompra
            .AsNoTracking()
            .Include(d => d.Proveedor)
            .Include(d => d.Usuario)
            .Include(d => d.UsuarioAnula)
            .Include(d => d.CompraDia)
            .Include(d => d.Items)
                .ThenInclude(i => i.CodigoProducto)
            .FirstOrDefaultAsync(d => d.Id == query.CompraId, ct);

        if (compra is null)
            throw new CompraNoEncontradaException();

        return new ObtenerDetalleCompraResponse(
            compra.Id,
            compra.CompraDiaId,
            compra.CompraDia.Estado.ToString(),
            compra.Proveedor.Nombre,
            compra.Usuario.Nombre,
            compra.FechaCompra,
            compra.FechaHora,
            compra.Total,
            compra.EstaAnulada,
            compra.NumeroComprobante,
            compra.TipoComprobante?.ToString(),
            compra.FechaComprobante,
            compra.UsuarioAnula?.Nombre,
            compra.FechaAnulacion,
            compra.MotivoAnulacion,
            compra.Items
                .Select(i => new DetalleItemCompraResponse(
                    i.Id,
                    i.ProductoId,
                    i.NombreProducto,
                    i.Cantidad,
                    i.PrecioCompra,
                    i.Subtotal,
                    i.CodigoProductoId,
                    i.CodigoProducto?.Codigo,
                    i.Factor
                ))
                .ToList()
        );
    }
}
