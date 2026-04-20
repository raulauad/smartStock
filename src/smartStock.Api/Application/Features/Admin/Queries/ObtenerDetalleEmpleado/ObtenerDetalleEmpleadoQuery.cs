using MediatR;
using smartStock.Shared.Dtos.Admin.ObtenerDetalleEmpleado;

namespace smartStock.Api.Application.Features.Admin.Queries.ObtenerDetalleEmpleado;

public sealed record ObtenerDetalleEmpleadoQuery(Guid EmpleadoId)
    : IRequest<ObtenerDetalleEmpleadoResponse>;
