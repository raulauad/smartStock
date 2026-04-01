using MediatR;
using smartStock.Application.Features.Commands.Usuarios;
using smartStock.Application.Features.Commands.Usuarios.DTOs;

namespace smartStock.Application.Features.Commands.Usuarios.AltaEmpleado;

public sealed record AltaEmpleadoCommand(
    string       Nombre,
    string       Email,
    string       Telefono,
    string       Dni,
    DireccionDto Direccion,
    string       Contrasena
) : IRequest<AltaEmpleadoResponse>;
