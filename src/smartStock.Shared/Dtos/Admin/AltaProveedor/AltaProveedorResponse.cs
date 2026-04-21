namespace smartStock.Shared.Dtos.Admin.AltaProveedor;

public sealed record AltaProveedorResponse(
    Guid    Id,
    string  Nombre,
    string  Email,
    string? Cuit
);
