namespace smartStock.Shared.Dtos.Admin.ObtenerListaCategorias;

public sealed record ObtenerListaCategoriasResponse(
    Guid    Id,
    string  Nombre,
    string? Descripcion,
    bool    EstaActivo,
    int     CantidadProductos
);
