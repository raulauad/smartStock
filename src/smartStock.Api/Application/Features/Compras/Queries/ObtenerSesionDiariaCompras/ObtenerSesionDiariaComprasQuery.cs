using MediatR;
using smartStock.Shared.Dtos.Compras.ObtenerSesionDiariaCompras;

namespace smartStock.Api.Application.Features.Compras.Queries.ObtenerSesionDiariaCompras;

public sealed record ObtenerSesionDiariaComprasQuery(
    DateTime? Fecha   // null = sesión del día actual
) : IRequest<ObtenerSesionDiariaComprasResponse>;
