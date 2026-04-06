using MediatR;
using Microsoft.AspNetCore.Identity;
using smartStock.Application.Common.Exceptions;
using smartStock.Application.Common.Exceptions.Auth;
using smartStock.Application.Common.Exceptions.Usuarios;
using smartStock.Domain.Models;

namespace smartStock.Application.Features.Commands.Usuarios.CambiarEstadoEmpleado;

public sealed class CambiarEstadoEmpleadoCommandHandler
    : IRequestHandler<CambiarEstadoEmpleadoCommand, CambiarEstadoEmpleadoResponse>
{
    private const string RolEmpleado = "Empleado";

    private readonly UserManager<Usuario> _userManager;

    public CambiarEstadoEmpleadoCommandHandler(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CambiarEstadoEmpleadoResponse> Handle(
        CambiarEstadoEmpleadoCommand command,
        CancellationToken            cancellationToken)
    {
        var usuario = await _userManager.FindByIdAsync(command.EmpleadoId.ToString());

        if (usuario is null)
            throw new UsuarioNoEncontradoException();

        // Solo se permite cambiar el estado de empleados, no de administradores
        if (!await _userManager.IsInRoleAsync(usuario, RolEmpleado))
            throw new AccesoNoPermitidoException();

        // FA: estado ya establecido (ej: desactivar a alguien ya suspendido)
        if (usuario.EstaActivo == command.EstaActivo)
            throw new EstadoUsuarioSinCambioException(command.EstaActivo);

        usuario.EstaActivo = command.EstaActivo;

        var resultado = await _userManager.UpdateAsync(usuario);

        if (!resultado.Succeeded)
            throw new IdentityException(resultado.Errors);

        return new CambiarEstadoEmpleadoResponse(usuario.Nombre, usuario.EstaActivo);
    }
}
