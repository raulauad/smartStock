using MediatR;
using smartStock.Shared.Dtos.Admin.ObtenerDetalleProveedor;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Queries.ObtenerDetalleProveedor;

public sealed record ObtenerDetalleProveedorQuery(Guid ProveedorId)
    : IRequest<ObtenerDetalleProveedorResponse>;
