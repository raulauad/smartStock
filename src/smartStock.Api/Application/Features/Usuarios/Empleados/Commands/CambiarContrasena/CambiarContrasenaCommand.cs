using MediatR;
using smartStock.Shared.Dtos.Empleados.CambiarContrasena;

namespace smartStock.Api.Application.Features.Usuarios.Empleados.Commands.CambiarContrasena;

public sealed record CambiarContrasenaCommand(
    string ContrasenaActual,
    string NuevaContrasena,
    string ConfirmacionContrasena
) : IRequest<CambiarContrasenaResponse>
{
    // Asignado por el controller desde el claim "sub" del JWT.
    // Nunca se toma del body para evitar que el usuario manipule su propio Id.
    public Guid UsuarioId { get; init; }
}
