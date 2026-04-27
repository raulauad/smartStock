namespace smartStock.Shared.Dtos.Compras.ObtenerAjustesStock;

public sealed record ObtenerAjustesStockResponse(
    int      Id,
    Guid     ProductoId,
    string   ProductoNombre,
    string   TipoMovimiento,
    decimal  Cantidad,
    string?  Observacion,
    string   UsuarioNombre,
    DateTime FechaHora
);
