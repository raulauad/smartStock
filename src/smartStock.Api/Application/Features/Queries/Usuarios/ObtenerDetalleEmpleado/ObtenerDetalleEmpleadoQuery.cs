using MediatR;

namespace smartStock.Api.Application.Features.Queries.Usuarios.ObtenerDetalleEmpleado;

public sealed record ObtenerDetalleEmpleadoQuery(Guid EmpleadoId)
    : IRequest<ObtenerDetalleEmpleadoResponse>;
