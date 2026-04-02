using MediatR;
using smartStock.Application.Features.Commands.Usuarios.DTOs;

namespace smartStock.Application.Features.Commands.Usuarios.IniciarSesion;

public sealed record IniciarSesionCommand(
    string Email,
    string Contrasena
) : IRequest<IniciarSesionResponse>;
