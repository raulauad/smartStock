namespace smartStock.Shared.Dtos.Compras.ObtenerListaCompras;

public sealed record ObtenerListaComprasResponse(
    int       Id,
    int       CompraDiaId,
    string    ProveedorNombre,
    string    UsuarioNombre,
    DateTime  FechaCompra,
    DateTime  FechaHora,
    decimal   Total,
    int       CantidadItems,
    bool      EstaAnulada,
    string?   NumeroComprobante,
    string?   TipoComprobante
);
