using MediatR;
using smartStock.Shared.Dtos.Ventas.ObtenerListaVentas;

namespace smartStock.Api.Application.Features.Ventas.Queries.ObtenerListaVentas;

public sealed record ObtenerListaVentasQuery(
    DateTime? FechaDesde,
    DateTime? FechaHasta,
    string?   FiltroFormaPago,
    Guid?     UsuarioRegistroId,
    string?   FiltroEstado,
    int?      NumeroComprobante,
    bool      EsAdmin
) : IRequest<List<ObtenerListaVentasResponse>>;
