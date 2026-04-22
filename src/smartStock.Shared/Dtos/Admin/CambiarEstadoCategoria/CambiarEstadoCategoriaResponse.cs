namespace smartStock.Shared.Dtos.Admin.CambiarEstadoCategoria;

public sealed record CambiarEstadoCategoriaResponse(
    string Nombre,
    bool   EstaActivo,
    int    ProductosReasignados
);
