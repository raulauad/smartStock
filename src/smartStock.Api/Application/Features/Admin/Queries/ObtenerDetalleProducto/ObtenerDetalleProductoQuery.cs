using MediatR;
using smartStock.Shared.Dtos.Admin.ObtenerDetalleProducto;

namespace smartStock.Api.Application.Features.Admin.Queries.ObtenerDetalleProducto;

public sealed record ObtenerDetalleProductoQuery(Guid ProductoId) : IRequest<ObtenerDetalleProductoResponse>;
