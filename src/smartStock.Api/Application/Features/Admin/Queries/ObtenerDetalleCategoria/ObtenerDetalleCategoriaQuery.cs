using MediatR;
using smartStock.Shared.Dtos.Admin.ObtenerDetalleCategoria;

namespace smartStock.Api.Application.Features.Admin.Queries.ObtenerDetalleCategoria;

public sealed record ObtenerDetalleCategoriaQuery(Guid CategoriaId)
    : IRequest<ObtenerDetalleCategoriaResponse>;
