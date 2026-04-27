namespace smartStock.Shared.Dtos.Compras.ObtenerSesionDiariaCompras;

public sealed record ObtenerSesionDiariaComprasResponse(
    int      Id,
    DateTime FechaSesion,
    string   Estado,
    string   UsuarioAperturaNombre,
    decimal  TotalVigente,
    int      CantidadVigentes,
    int      CantidadAnuladas,
    List<ResumenCompraResponse> Compras
);

public sealed record ResumenCompraResponse(
    int      Id,
    string   ProveedorNombre,
    DateTime FechaCompra,
    decimal  Total,
    int      CantidadItems,
    bool     EstaAnulada,
    string?  NumeroComprobante,
    string?  TipoComprobante
);
