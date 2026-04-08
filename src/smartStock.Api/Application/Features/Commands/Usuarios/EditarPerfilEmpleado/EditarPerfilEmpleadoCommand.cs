using MediatR;
using smartStock.Api.Application.Features.Commands.Usuarios;

namespace smartStock.Api.Application.Features.Commands.Usuarios.EditarPerfilEmpleado;

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
