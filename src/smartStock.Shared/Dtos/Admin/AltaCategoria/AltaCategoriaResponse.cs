namespace smartStock.Shared.Dtos.Admin.AltaCategoria;

public sealed record AltaCategoriaResponse(
    Guid     Id,
    string   Nombre,
    string?  Descripcion,
    DateTime FechaAlta
);
