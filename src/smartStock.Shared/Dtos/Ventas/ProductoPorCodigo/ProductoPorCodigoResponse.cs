namespace smartStock.Shared.Dtos.Ventas.ProductoPorCodigo;

public sealed record ProductoPorCodigoResponse(
    Guid     ProductoId,
    string   NombreProducto,
    decimal  PrecioVenta,
    decimal? PrecioCosto,       // null para empleados no administradores
    decimal  StockActual,
    string   UnidadMedida,
    bool     EstaActivo,
    decimal  Factor,
    Guid     CodigoProductoId,
    string   TipoCodigo
);
