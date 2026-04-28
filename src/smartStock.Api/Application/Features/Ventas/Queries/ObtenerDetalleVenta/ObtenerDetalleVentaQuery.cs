using MediatR;
using smartStock.Shared.Dtos.Ventas.ObtenerDetalleVenta;

namespace smartStock.Api.Application.Features.Ventas.Queries.ObtenerDetalleVenta;

public sealed record ObtenerDetalleVentaQuery(int VentaId, bool EsAdmin)
    : IRequest<ObtenerDetalleVentaResponse?>;
