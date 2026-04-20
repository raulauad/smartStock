using MediatR;
using smartStock.Shared.Dtos.Admin.AltaEmpleado;
using smartStock.Shared.Dtos.Shared;

namespace smartStock.Api.Application.Features.Admin.Commands.AltaEmpleado;

public sealed record AltaEmpleadoCommand(
    string       Nombre,
    string       Email,
    string       Telefono,
    string       Dni,
    DireccionDto Direccion,
    string       Contrasena
) : IRequest<AltaEmpleadoResponse>;
