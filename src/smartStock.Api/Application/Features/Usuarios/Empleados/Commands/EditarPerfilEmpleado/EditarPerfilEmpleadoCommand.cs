using MediatR;
using smartStock.Shared.Dtos.Empleados.EditarPerfilEmpleado;
using smartStock.Shared.Dtos.Shared;

namespace smartStock.Api.Application.Features.Usuarios.Empleados.Commands.EditarPerfilEmpleado;

public sealed record EditarPerfilEmpleadoCommand(
    string       Nombre,
    string       Email,
    string       Telefono,
    string       Dni,
    DireccionDto Direccion
) : IRequest<EditarPerfilEmpleadoResponse>
{
    // Asignado por el controller desde el claim "sub" del JWT.
    // Nunca se toma del body para evitar que el usuario manipule su propio Id.
    public Guid UsuarioId { get; init; }
}
