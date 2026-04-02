using MediatR;
using Microsoft.AspNetCore.Identity;
using smartStock.Application.Common.Exceptions.Auth;
using smartStock.Application.Common.Interfaces;
using smartStock.Application.Features.Commands.Usuarios.DTOs;
using smartStock.Domain.Models;

namespace smartStock.Application.Features.Commands.Usuarios.IniciarSesion;

public sealed class IniciarSesionCommandHandler
    : IRequestHandler<IniciarSesionCommand, IniciarSesionResponse>
{
    private readonly UserManager<Usuario> _userManager;
    private readonly IJwtTokenService     _jwtTokenService;

    public IniciarSesionCommandHandler(
        UserManager<Usuario> userManager,
        IJwtTokenService     jwtTokenService)
    {
        _userManager     = userManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<IniciarSesionResponse> Handle(
        IniciarSesionCommand command,
        CancellationToken    cancellationToken)
    {
        var usuario = await _userManager.FindByEmailAsync(command.Email);

        // FA1: usuario no existe o contraseña incorrecta → mismo error (evita enumeración)
        if (usuario is null || !await _userManager.CheckPasswordAsync(usuario, command.Contrasena))
            throw new CredencialesInvalidasException();

        // FA2: cuenta desactivada por el administrador
        if (!usuario.EstaActivo)
            throw new CuentaInactivaException();

        var roles               = await _userManager.GetRolesAsync(usuario);
        var (token, expiracion) = _jwtTokenService.GenerarToken(usuario, roles);

        return new IniciarSesionResponse(usuario.Id, usuario.Nombre, usuario.Email!, roles[0], token, expiracion);
    }
}
