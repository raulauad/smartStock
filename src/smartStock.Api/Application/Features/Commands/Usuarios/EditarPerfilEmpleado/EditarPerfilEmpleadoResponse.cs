namespace smartStock.Api.Application.Features.Commands.Usuarios.EditarPerfilEmpleado;

public sealed record EditarPerfilEmpleadoResponse(
    string Nombre,
    string Email,
    string Telefono,
    string Dni
);
