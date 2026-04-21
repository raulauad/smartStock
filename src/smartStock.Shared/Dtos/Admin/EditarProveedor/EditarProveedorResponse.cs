namespace smartStock.Shared.Dtos.Admin.EditarProveedor;

public sealed record EditarProveedorResponse(
    Guid    Id,
    string  Nombre,
    string  Email,
    string? Cuit
);
