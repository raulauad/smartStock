using MediatR;
using smartStock.Api.Application.Features.Commands.Usuarios;
using smartStock.Api.Application.Features.Commands.Usuarios.DTOs;

namespace smartStock.Api.Application.Features.Commands.Usuarios.RegistrarAdmin;

public sealed record RegistrarAdministradorCommand(
    string       Nombre,
    string       Email,
    string       Telefono,
    string       Dni,
    DireccionDto Direccion,
    string       Contrasena
) : IRequest<RegistrarAdministradorResponse>;
