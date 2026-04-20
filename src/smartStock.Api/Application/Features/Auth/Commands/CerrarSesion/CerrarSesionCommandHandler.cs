using MediatR;
using smartStock.Api.Application.Common.Interfaces.Repositories;

namespace smartStock.Api.Application.Features.Auth.Commands.CerrarSesion;

public sealed class CerrarSesionCommandHandler : IRequestHandler<CerrarSesionCommand>
{
    private readonly ITokenRevocadoRepository _tokenRevocadoRepository;

    public CerrarSesionCommandHandler(ITokenRevocadoRepository tokenRevocadoRepository)
        => _tokenRevocadoRepository = tokenRevocadoRepository;

    public Task Handle(CerrarSesionCommand command, CancellationToken cancellationToken)
        => _tokenRevocadoRepository.RevocarAsync(
               command.Jti,
               command.Expiracion,
               command.UsuarioId,
               cancellationToken);
}
