namespace smartStock.Shared.Dtos.Admin.ObtenerDetalleProveedor;

public sealed record ObtenerDetalleProveedorResponse(
    Guid                       Id,
    string                     Nombre,
    string?                    Cuit,
    string                     Telefono,
    string                     Email,
    DireccionProveedorResponse Direccion,
    string?                    Observaciones,
    bool                       EstaActivo,
    DateTime                   FechaAlta,
    string                     NombreAdmin
);

public sealed record DireccionProveedorResponse(
    string Pais,
    string Provincia,
    string Localidad,
    string CodigoPostal,
    string Calle,
    string Numero
);
