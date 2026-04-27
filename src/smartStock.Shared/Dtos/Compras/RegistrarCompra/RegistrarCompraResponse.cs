namespace smartStock.Shared.Dtos.Compras.RegistrarCompra;

public sealed record RegistrarCompraResponse(
    int       Id,
    int       CompraDiaId,
    Guid      ProveedorId,
    string    ProveedorNombre,
    DateTime  FechaCompra,
    DateTime  FechaHora,
    decimal   Total,
    string?   NumeroComprobante,
    string?   TipoComprobante,
    DateTime? FechaComprobante,
    List<ItemCompraResponse> Items
);

public sealed record ItemCompraResponse(
    int      Id,
    Guid     ProductoId,
    string   NombreProducto,
    decimal  Cantidad,
    decimal  PrecioCompra,
    decimal  Subtotal,
    Guid?    CodigoProductoId,
    decimal? Factor
);
