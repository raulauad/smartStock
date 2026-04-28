namespace smartStock.Shared.Dtos.Ventas.ObtenerListaVentas;

public sealed record ObtenerListaVentasResponse(
    int      Id,
    int      VentaDiaId,
    int      NumeroComprobante,
    DateTime FechaHora,
    string   UsuarioNombre,
    decimal  Total,
    int      CantidadItems,
    string   FormaPago,
    bool     EstaAnulada,
    decimal? GananciaBruta   // null para empleados no administradores
);
