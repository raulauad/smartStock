using MediatR;
using Microsoft.AspNetCore.Identity;
using smartStock.Api.Application.Common.Exceptions;
using smartStock.Api.Application.Common.Exceptions.Auth;
using smartStock.Api.Application.Common.Exceptions.Usuarios;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Application.Features.Commands.Usuarios.CambiarContrasena;

public sealed class CambiarContrasenaCommandHandler
    : IRequestHandler<CambiarContrasenaCommand, CambiarContrasenaResponse>
{
    private readonly UserManager<Usuario> _userManager;

    public CambiarContrasenaCommandHandler(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CambiarContrasenaResponse> Handle(
        CambiarContrasenaCommand command,
        CancellationToken        cancellationToken)
    {
        var usuario = await _userManager.FindByIdAsync(command.UsuarioId.ToString());

        if (usuario is null)
            throw new UsuarioNoEncontradoException();

        // FA1: contraseña actual incorrecta
        var contrasenaValida = await _userManager.CheckPasswordAsync(usuario, command.ContrasenaActual);

        if (!contrasenaValida)
            throw new CredencialesInvalidasException();

        var resultado = await _userManager.ChangePasswordAsync(
            usuario,
            command.ContrasenaActual,
            command.NuevaContrasena);

        if (!resultado.Succeeded)
            throw new IdentityException(resultado.Errors);

        return new CambiarContrasenaResponse("La contraseña fue actualizada exitosamente.");
    }
}
