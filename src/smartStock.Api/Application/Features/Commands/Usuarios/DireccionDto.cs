namespace smartStock.Api.Application.Features.Commands.Usuarios;

public sealed record DireccionDto(
    string Pais,
    string Provincia,
    string Localidad,
    string CodigoPostal,
    string Calle,
    string Numero
);
