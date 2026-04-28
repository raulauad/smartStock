using MediatR;
using smartStock.Shared.Dtos.Admin.CambiarEstadoEmpleado;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Commands.CambiarEstadoEmpleado;

public sealed record CambiarEstadoEmpleadoCommand(
    bool EstaActivo
) : IRequest<CambiarEstadoEmpleadoResponse>
{
    // Asignado por el controller desde el parámetro de ruta {id}.
    // Nunca se toma del body para evitar que se manipule el id del empleado.
    public Guid EmpleadoId { get; init; }
}
