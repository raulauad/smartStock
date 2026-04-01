namespace smartStock.Application.Features.Commands.Usuarios.DTOs;

public sealed record IniciarSesionAdminResponse(
    Guid     Id,
    string   Nombre,
    string   Email,
    string   Token,
    DateTime Expiracion
);
