namespace smartStock.Application.Features.Queries.Usuarios.ObtenerPerfilAdmin;

public sealed record ObtenerPerfilAdminResponse(
    Guid   Id,
    string Nombre,
    string Email,
    string Telefono,
    string Dni
);
