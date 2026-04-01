namespace smartStock.Application.Features.Commands.Usuarios.DTOs;

public sealed record RegistrarAdministradorResponse(
    Guid   Id,
    string Nombre,
    string Email
);
