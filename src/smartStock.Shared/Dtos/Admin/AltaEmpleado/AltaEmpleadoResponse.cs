namespace smartStock.Shared.Dtos.Admin.AltaEmpleado;

public sealed record AltaEmpleadoResponse(
    Guid   Id,
    string Nombre,
    string Email
);
