namespace smartStock.Shared.Dtos.Admin.ObtenerDetalleCategoria;

public sealed record ObtenerDetalleCategoriaResponse(
    Guid     Id,
    string   Nombre,
    string?  Descripcion,
    bool     EstaActivo,
    DateTime FechaAlta,
    string   NombreAdmin,
    int      CantidadProductos
);
