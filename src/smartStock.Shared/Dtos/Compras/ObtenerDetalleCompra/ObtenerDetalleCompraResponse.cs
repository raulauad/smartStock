namespace smartStock.Shared.Dtos.Compras.ObtenerDetalleCompra;

public sealed record ObtenerDetalleCompraResponse(
    int       Id,
    int       CompraDiaId,
    string    EstadoSesion,
    string    ProveedorNombre,
    string    UsuarioNombre,
    DateTime  FechaCompra,
    DateTime  FechaHora,
    decimal   Total,
    bool      EstaAnulada,
    string?   NumeroComprobante,
    string?   TipoComprobante,
    DateTime? FechaComprobante,
    string?   UsuarioAnulaNombre,
    DateTime? FechaAnulacion,
    string?   MotivoAnulacion,
    List<DetalleItemCompraResponse> Items
);

public sealed record DetalleItemCompraResponse(
    int      Id,
    Guid     ProductoId,
    string   NombreProducto,
    decimal  Cantidad,
    decimal  PrecioCompra,
    decimal  Subtotal,
    Guid?    CodigoProductoId,
    string?  CodigoCodigo,
    decimal? Factor
);
