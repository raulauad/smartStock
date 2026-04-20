using MediatR;
using smartStock.Shared.Dtos.Auth.IniciarSesion;

namespace smartStock.Api.Application.Features.Auth.Commands.IniciarSesion;

public sealed record IniciarSesionCommand(
    string Email,
    string Contrasena
) : IRequest<IniciarSesionResponse>;
