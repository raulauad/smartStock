namespace smartStock.Shared.Dtos.Ventas.RegistrarVenta;

public sealed record RegistrarVentaResponse(
    int                      Id,
    int                      VentaDiaId,
    int                      NumeroComprobante,
    DateTime                 FechaHora,
    decimal                  Total,
    string                   FormaPago,
    decimal?                 MontoRecibido,
    List<ItemVentaResponse>  Items
);

public sealed record ItemVentaResponse(
    int      Id,
    Guid     ProductoId,
    string   NombreProducto,
    decimal  Cantidad,
    decimal  PrecioVenta,
    decimal  Subtotal,
    Guid?    CodigoProductoId,
    decimal? Factor
);
