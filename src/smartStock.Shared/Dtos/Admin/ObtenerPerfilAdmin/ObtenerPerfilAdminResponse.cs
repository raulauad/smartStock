namespace smartStock.Shared.Dtos.Admin.ObtenerPerfilAdmin;

public sealed record ObtenerPerfilAdminResponse(
    Guid   Id,
    string Nombre,
    string Email,
    string Telefono,
    string Dni
);
