using MediatR;
using smartStock.Api.Application.Common.Exceptions.Auth;
using smartStock.Api.Application.Common.Exceptions.Usuarios;
using smartStock.Api.Application.Common.Interfaces.Auth;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Shared.Dtos.Empleados.CambiarContrasena;

namespace smartStock.Api.Application.Features.Empleados.Commands.CambiarContrasena;

public sealed class CambiarContrasenaCommandHandler
    : IRequestHandler<CambiarContrasenaCommand, CambiarContrasenaResponse>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher    _passwordHasher;

    public CambiarContrasenaCommandHandler(
        IUsuarioRepository usuarioRepository,
        IPasswordHasher    passwordHasher)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher    = passwordHasher;
    }

    public async Task<CambiarContrasenaResponse> Handle(
        CambiarContrasenaCommand command,
        CancellationToken        cancellationToken)
    {
        var usuario = await _usuarioRepository.ObtenerPorIdAsync(command.UsuarioId, cancellationToken)
            ?? throw new UsuarioNoEncontradoException();

        // FA1: contraseña actual incorrecta
        if (!_passwordHasher.Verificar(command.ContrasenaActual, usuario.ContrasenaHash))
            throw new CredencialesInvalidasException();

        usuario.ContrasenaHash = _passwordHasher.Hashear(command.NuevaContrasena);

        await _usuarioRepository.ActualizarAsync(usuario, cancellationToken);

        return new CambiarContrasenaResponse("La contraseña fue actualizada exitosamente.");
    }
}
