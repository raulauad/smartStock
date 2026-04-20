namespace smartStock.Shared.Dtos.Admin.RegistrarAdmin;

public sealed record RegistrarAdministradorResponse(
    Guid   Id,
    string Nombre,
    string Email
);
