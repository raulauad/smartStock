using smartStock.Shared.Dtos.Admin.Productos;

namespace smartStock.Shared.Dtos.Admin.ObtenerDetalleProducto;

public sealed record ObtenerDetalleProductoResponse(
    Guid                         Id,
    string                       Nombre,
    string?                      Descripcion,
    string                       Categoria,
    string                       UnidadMedida,
    decimal                      PrecioCosto,
    decimal                      PrecioVenta,
    decimal                      StockActual,
    decimal                      StockMinimo,
    bool                         EstaActivo,
    DateTime                     FechaAlta,
    string                       AdminAlta,
    List<CodigoProductoResponse> Codigos
);
