namespace smartStock.Application.Features.Commands.Usuarios.DTOs;

public sealed record IniciarSesionResponse(
    Guid     Id,
    string   Nombre,
    string   Email,
    string   Rol,
    string   Token,
    DateTime Expiracion
);
