namespace smartStock.Shared.Dtos.Ventas.AnularVenta;

public sealed record AnularVentaResponse(
    int      Id,
    bool     EstaAnulada,
    DateTime FechaAnulacion,
    string   MotivoAnulacion,
    string   UsuarioAnulaNombre
);
