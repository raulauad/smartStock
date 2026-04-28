using MediatR;
using smartStock.Shared.Dtos.Admin.RegistrarAdmin;
using smartStock.Shared.Dtos.Shared;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Commands.RegistrarAdmin;

public sealed record RegistrarAdministradorCommand(
    string       Nombre,
    string       Email,
    string       Telefono,
    string       Dni,
    DireccionDto Direccion,
    string       Contrasena
) : IRequest<RegistrarAdministradorResponse>;
