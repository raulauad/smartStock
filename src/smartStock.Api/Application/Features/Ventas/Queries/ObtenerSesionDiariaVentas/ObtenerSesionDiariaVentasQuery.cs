using MediatR;
using smartStock.Shared.Dtos.Ventas.ObtenerSesionDiariaVentas;

namespace smartStock.Api.Application.Features.Ventas.Queries.ObtenerSesionDiariaVentas;

public sealed record ObtenerSesionDiariaVentasQuery(DateTime? Fecha, bool EsAdmin)
    : IRequest<ObtenerSesionDiariaVentasResponse?>;
