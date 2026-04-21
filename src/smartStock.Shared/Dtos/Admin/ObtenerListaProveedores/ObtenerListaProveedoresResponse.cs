namespace smartStock.Shared.Dtos.Admin.ObtenerListaProveedores;

public sealed record ObtenerListaProveedoresResponse(
    Guid    Id,
    string  Nombre,
    string? Cuit,
    string  Telefono,
    string  Email,
    bool    EstaActivo
);
