using MediatR;

namespace smartStock.Api.Application.Features.Admin.Commands.EliminarEmpleado;

public sealed record EliminarEmpleadoCommand : IRequest
{
    // Asignado por el controller desde el parámetro de ruta {id}.
    // Nunca se toma del body para evitar que se manipule el id del empleado.
    public Guid EmpleadoId { get; init; }
}
