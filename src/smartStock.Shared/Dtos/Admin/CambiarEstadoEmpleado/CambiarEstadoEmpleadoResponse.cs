namespace smartStock.Shared.Dtos.Admin.CambiarEstadoEmpleado;

public sealed record CambiarEstadoEmpleadoResponse(
    string Nombre,
    bool   EstaActivo
);
