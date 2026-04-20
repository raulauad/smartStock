namespace smartStock.Shared.Dtos.Empleados.EditarPerfilEmpleado;

public sealed record EditarPerfilEmpleadoResponse(
    string Nombre,
    string Email,
    string Telefono,
    string Dni
);
