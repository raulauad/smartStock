namespace smartStock.Shared.Dtos.Compras.AjusteManualStock;

public sealed record AjusteManualStockResponse(
    int      MovimientoId,
    Guid     ProductoId,
    string   ProductoNombre,
    string   TipoAjuste,
    decimal  Cantidad,
    decimal  StockAnterior,
    decimal  StockResultante,
    string   Motivo,
    DateTime FechaHora
);
