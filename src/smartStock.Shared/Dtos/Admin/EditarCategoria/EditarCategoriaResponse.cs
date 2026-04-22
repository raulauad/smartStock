namespace smartStock.Shared.Dtos.Admin.EditarCategoria;

public sealed record EditarCategoriaResponse(
    Guid    Id,
    string  Nombre,
    string? Descripcion
);
