namespace smartStock.Shared.Dtos.Ventas.ObtenerSesionDiariaVentas;

public sealed record ObtenerSesionDiariaVentasResponse(
    int                          Id,
    DateTime                     FechaSesion,
    string                       Estado,
    string                       UsuarioAperturaNombre,
    decimal                      TotalVigente,
    int                          CantidadVigentes,
    int                          CantidadAnuladas,
    Dictionary<string, decimal>  TotalPorFormaPago,   // solo ventas vigentes
    decimal?                     GananciaBruta,        // null para empleados no administradores
    List<ResumenVentaResponse>   Ventas
);

public sealed record ResumenVentaResponse(
    int      Id,
    int      NumeroComprobante,
    DateTime FechaHora,
    string   UsuarioNombre,
    decimal  Total,
    int      CantidadItems,
    string   FormaPago,
    bool     EstaAnulada
);
