using MediatR;

namespace smartStock.Application.Features.Queries.Usuarios.ObtenerDetalleEmpleado;

public sealed record ObtenerDetalleEmpleadoQuery(Guid EmpleadoId)
    : IRequest<ObtenerDetalleEmpleadoResponse>;
