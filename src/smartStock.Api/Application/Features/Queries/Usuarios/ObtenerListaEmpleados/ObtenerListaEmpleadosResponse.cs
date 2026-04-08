namespace smartStock.Api.Application.Features.Queries.Usuarios.ObtenerListaEmpleados;

public sealed record ObtenerListaEmpleadosResponse(
    Guid   Id,
    string Nombre,
    string Email,
    bool   EstaActivo
);
