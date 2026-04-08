namespace smartStock.Api.Application.Features.Queries.Usuarios.ObtenerDetalleEmpleado;

public sealed record ObtenerDetalleEmpleadoResponse(
    Guid                  Id,
    string                Nombre,
    string                Email,
    string                Telefono,
    string                Dni,
    DireccionEmpleadoResponse Direccion
);

public sealed record DireccionEmpleadoResponse(
    string Pais,
    string Provincia,
    string Localidad,
    string CodigoPostal,
    string Calle,
    string Numero
);
