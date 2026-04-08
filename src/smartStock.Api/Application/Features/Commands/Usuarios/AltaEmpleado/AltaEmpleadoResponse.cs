namespace smartStock.Api.Application.Features.Commands.Usuarios.DTOs;

public sealed record AltaEmpleadoResponse(
    Guid   Id,
    string Nombre,
    string Email
);
