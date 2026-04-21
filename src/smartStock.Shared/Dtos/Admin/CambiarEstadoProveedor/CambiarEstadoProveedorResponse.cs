namespace smartStock.Shared.Dtos.Admin.CambiarEstadoProveedor;

public sealed record CambiarEstadoProveedorResponse(
    string Nombre,
    bool   EstaActivo
);
