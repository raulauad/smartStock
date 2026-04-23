using smartStock.Shared.Dtos.Admin.Productos;

namespace smartStock.Shared.Dtos.Admin.AltaProducto;

public sealed record AltaProductoResponse(
    Guid                         Id,
    string                       Nombre,
    string                       Categoria,
    string                       UnidadMedida,
    decimal                      PrecioCosto,
    decimal                      PrecioVenta,
    decimal                      StockInicial,
    decimal                      StockMinimo,
    bool                         EstaActivo,
    DateTime                     FechaAlta,
    List<CodigoProductoResponse> Codigos
);
