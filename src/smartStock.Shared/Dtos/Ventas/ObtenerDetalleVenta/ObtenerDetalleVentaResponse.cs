namespace smartStock.Shared.Dtos.Ventas.ObtenerDetalleVenta;

public sealed record ObtenerDetalleVentaResponse(
    int                               Id,
    int                               VentaDiaId,
    int                               NumeroComprobante,
    DateTime                          FechaHora,
    string                            UsuarioNombre,
    decimal                           Total,
    string                            FormaPago,
    decimal?                          MontoRecibido,
    bool                              EstaAnulada,
    DateTime?                         FechaAnulacion,
    string?                           MotivoAnulacion,
    string?                           UsuarioAnulaNombre,
    List<ItemDetalleVentaResponse>    Items
);

public sealed record ItemDetalleVentaResponse(
    int      Id,
    string   NombreProducto,
    decimal  Cantidad,
    decimal  PrecioVenta,
    decimal  Subtotal,
    Guid?    CodigoProductoId,
    decimal? Factor,
    decimal? PrecioCosto,     // null para empleados no administradores
    decimal? GananciaTotal    // null para empleados no administradores
);
