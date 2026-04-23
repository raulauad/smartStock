namespace smartStock.Shared.Dtos.Admin.ObtenerListaProductos;

public sealed record ObtenerListaProductosResponse(
    Guid    Id,
    string  Nombre,
    string  Categoria,
    string  UnidadMedida,
    decimal PrecioVenta,
    decimal StockActual,
    decimal StockMinimo,
    bool    EstaActivo,
    bool    AlertaStockBajo
);
