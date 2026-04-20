namespace smartStock.Shared.Dtos.Shared;

public sealed record DireccionDto(
    string Pais,
    string Provincia,
    string Localidad,
    string CodigoPostal,
    string Calle,
    string Numero
);
