namespace smartStock.Shared.Dtos.Admin.EditarCodigoProducto;

public sealed record EditarCodigoProductoResponse(
    Guid    Id,
    string  Codigo,
    string  Tipo,
    decimal Factor,
    string? Descripcion
);
