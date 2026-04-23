namespace smartStock.Shared.Dtos.Admin.Productos;

public sealed record CodigoProductoResponse(
    Guid    Id,
    string  Codigo,
    string  Tipo,
    decimal Factor,
    string? Descripcion
);
