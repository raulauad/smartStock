using MediatR;

namespace smartStock.Api.Application.Features.Auth.Commands.CerrarSesion;

public sealed record CerrarSesionCommand : IRequest
{
    public string   Jti        { get; init; } = string.Empty;
    public DateTime Expiracion { get; init; }
    public Guid     UsuarioId  { get; init; }
}
