using MediatR;
using smartStock.Api.Application.Features.Commands.Usuarios.DTOs;

namespace smartStock.Api.Application.Features.Commands.Usuarios.IniciarSesion;

public sealed record IniciarSesionCommand(
    string Email,
    string Contrasena
) : IRequest<IniciarSesionResponse>;
