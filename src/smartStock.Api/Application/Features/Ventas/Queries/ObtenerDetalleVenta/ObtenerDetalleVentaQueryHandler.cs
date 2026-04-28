using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Infrastructure.Persistence;
using smartStock.Shared.Dtos.Ventas.ObtenerDetalleVenta;

namespace smartStock.Api.Application.Features.Ventas.Queries.ObtenerDetalleVenta;

public sealed class ObtenerDetalleVentaQueryHandler
    : IRequestHandler<ObtenerDetalleVentaQuery, ObtenerDetalleVentaResponse?>
{
    private readonly AppDbContext _db;

    public ObtenerDetalleVentaQueryHandler(AppDbContext db) => _db = db;

    public async Task<ObtenerDetalleVentaResponse?> Handle(ObtenerDetalleVentaQuery query, CancellationToken ct)
    {
        var venta = await _db.DetallesVenta
            .AsNoTracking()
            .Include(d => d.Usuario)
            .Include(d => d.UsuarioAnula)
            .Include(d => d.Items)
            .FirstOrDefaultAsync(d => d.Id == query.VentaId, ct);

        if (venta is null)
            return null;

        var esAdmin = query.EsAdmin;

        var items = venta.Items.Select(i => new ItemDetalleVentaResponse(
            i.Id,
            i.NombreProducto,
            i.Cantidad,
            i.PrecioVenta,
            i.Subtotal,
            i.CodigoProductoId,
            i.Factor,
            esAdmin ? i.PrecioCosto        : null,
            esAdmin ? i.GananciaTotal      : null
        )).ToList();

        return new ObtenerDetalleVentaResponse(
            venta.Id,
            venta.VentaDiaId,
            venta.NumeroComprobante,
            venta.FechaHora,
            venta.Usuario.Nombre,
            venta.Total,
            venta.FormaPago.ToString(),
            venta.MontoRecibido,
            venta.EstaAnulada,
            venta.FechaAnulacion,
            venta.MotivoAnulacion,
            venta.UsuarioAnula?.Nombre,
            items
        );
    }
}
