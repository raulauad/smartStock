namespace smartStock.Shared.Dtos.Admin.EditarProducto;

public sealed record EditarProductoResponse(
    Guid    Id,
    string  Nombre,
    string  Categoria,
    string  UnidadMedida,
    decimal PrecioCosto,
    decimal PrecioVenta,
    decimal StockMinimo
);
