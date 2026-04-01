using MediatR;
using Microsoft.AspNetCore.Identity;
using smartStock.Application.Common.Exceptions.Auth;
using smartStock.Application.Common.Interfaces;
using smartStock.Application.Features.Commands.Usuarios.DTOs;
using smartStock.Domain.Models;

namespace smartStock.Application.Features.Commands.Usuarios.IniciarSesion;

public sealed class IniciarSesionAdminCommandHandler
    : IRequestHandler<IniciarSesionAdminCommand, IniciarSesionAdminResponse>
{
    private const string RolAdministrador = "Administrador";

    private readonly UserManager<Usuario> _userManager;
    private readonly IJwtTokenService     _jwtTokenService;

    public IniciarSesionAdminCommandHandler(
        UserManager<Usuario> userManager,
        IJwtTokenService     jwtTokenService)
    {
        _userManager     = userManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<IniciarSesionAdminResponse> Handle(
        IniciarSesionAdminCommand command,
        CancellationToken         cancellationToken)
    {
        var usuario = await _userManager.FindByEmailAsync(command.Email);

        // FA1: usuario no existe o contraseña incorrecta → mismo error (evita enumeración)
        if (usuario is null || !await _userManager.CheckPasswordAsync(usuario, command.Contrasena))
            throw new CredencialesInvalidasException();

        // FA1: el usuario existe pero no es administrador
        if (!await _userManager.IsInRoleAsync(usuario, RolAdministrador))
            throw new CredencialesInvalidasException();

        var roles                    = await _userManager.GetRolesAsync(usuario);
        var (token, expiracion)      = _jwtTokenService.GenerarToken(usuario, roles);

        return new IniciarSesionAdminResponse(usuario.Id, usuario.Nombre, usuario.Email!, token, expiracion);
    }
}
