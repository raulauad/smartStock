namespace smartStock.Application.Features.Queries.Usuarios.ObtenerListaEmpleados;

public sealed record ObtenerListaEmpleadosResponse(
    Guid   Id,
    string Nombre,
    string Email,
    bool   EstaActivo
);
