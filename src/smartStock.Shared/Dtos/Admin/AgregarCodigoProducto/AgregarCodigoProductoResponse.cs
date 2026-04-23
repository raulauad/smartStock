using smartStock.Shared.Dtos.Admin.Productos;

namespace smartStock.Shared.Dtos.Admin.AgregarCodigoProducto;

public sealed record AgregarCodigoProductoResponse(
    Guid                         ProductoId,
    string                       NombreProducto,
    List<CodigoProductoResponse> Codigos
);
