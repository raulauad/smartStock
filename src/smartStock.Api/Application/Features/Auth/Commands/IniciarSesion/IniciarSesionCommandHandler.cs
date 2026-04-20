using MediatR;
using smartStock.Api.Application.Common.Exceptions.Auth;
using smartStock.Api.Application.Common.Interfaces.Auth;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Shared.Dtos.Auth.IniciarSesion;

namespace smartStock.Api.Application.Features.Auth.Commands.IniciarSesion;

public sealed class IniciarSesionCommandHandler
    : IRequestHandler<IniciarSesionCommand, IniciarSesionResponse>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher    _passwordHasher;
    private readonly IJwtTokenService   _jwtTokenService;

    public IniciarSesionCommandHandler(
        IUsuarioRepository usuarioRepository,
        IPasswordHasher    passwordHasher,
        IJwtTokenService   jwtTokenService)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher    = passwordHasher;
        _jwtTokenService   = jwtTokenService;
    }

    public async Task<IniciarSesionResponse> Handle(
        IniciarSesionCommand command,
        CancellationToken    cancellationToken)
    {
        var usuario = await _usuarioRepository.ObtenerPorEmailAsync(command.Email, cancellationToken);

        // FA1: usuario no existe o contraseña incorrecta → mismo error (evita enumeración)
        if (usuario is null || !_passwordHasher.Verificar(command.Contrasena, usuario.ContrasenaHash))
            throw new CredencialesInvalidasException();

        // FA2: cuenta desactivada por el administrador
        if (!usuario.EstaActivo)
            throw new CuentaInactivaException();

        var roles               = usuario.Roles.Select(r => r.Rol.ToString()).ToList();
        var (token, expiracion) = _jwtTokenService.GenerarToken(usuario, roles);

        return new IniciarSesionResponse(usuario.Id, usuario.Nombre, usuario.Email, roles[0], token, expiracion);
    }
}
