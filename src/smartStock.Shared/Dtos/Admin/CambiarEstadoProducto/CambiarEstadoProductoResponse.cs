namespace smartStock.Shared.Dtos.Admin.CambiarEstadoProducto;

public sealed record CambiarEstadoProductoResponse(
    Guid    Id,
    string  Nombre,
    bool    EstaActivo,
    decimal StockActual
);
