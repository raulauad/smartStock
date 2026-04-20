namespace smartStock.Shared.Dtos.Auth.IniciarSesion;

public sealed record IniciarSesionResponse(
    Guid     Id,
    string   Nombre,
    string   Email,
    string   Rol,
    string   Token,
    DateTime Expiracion
);
