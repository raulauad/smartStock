namespace smartStock.Shared.Dtos.Admin.ObtenerListaEmpleados;

public sealed record ObtenerListaEmpleadosResponse(
    Guid   Id,
    string Nombre,
    string Email,
    bool   EstaActivo
);
