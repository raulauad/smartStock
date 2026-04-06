namespace smartStock.Application.Features.Commands.Usuarios.CambiarEstadoEmpleado;

public sealed record CambiarEstadoEmpleadoResponse(
    string Nombre,
    bool   EstaActivo
);
