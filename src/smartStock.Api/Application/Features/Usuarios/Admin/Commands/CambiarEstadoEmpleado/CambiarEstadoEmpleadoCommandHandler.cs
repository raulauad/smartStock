using MediatR;
using smartStock.Api.Application.Common.Exceptions;
using smartStock.Api.Application.Common.Exceptions.Auth;
using smartStock.Api.Application.Common.Exceptions.Usuarios;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Enums;
using smartStock.Shared.Dtos.Admin.CambiarEstadoEmpleado;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Commands.CambiarEstadoEmpleado;

public sealed class CambiarEstadoEmpleadoCommandHandler
    : IRequestHandler<CambiarEstadoEmpleadoCommand, CambiarEstadoEmpleadoResponse>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public CambiarEstadoEmpleadoCommandHandler(IUsuarioRepository usuarioRepository)
        => _usuarioRepository = usuarioRepository;

    public async Task<CambiarEstadoEmpleadoResponse> Handle(
        CambiarEstadoEmpleadoCommand command,
        CancellationToken            cancellationToken)
    {
        var usuario = await _usuarioRepository.ObtenerPorIdAsync(command.EmpleadoId, cancellationToken)
            ?? throw new UsuarioNoEncontradoException();

        // Solo se permite cambiar el estado de empleados, no de administradores
        if (!usuario.Roles.Any(r => r.Rol == Rol.Empleado))
            throw new AccesoNoPermitidoException();

        // FA: estado ya establecido
        if (usuario.EstaActivo == command.EstaActivo)
            throw new EstadoUsuarioSinCambioException(command.EstaActivo);

        usuario.EstaActivo = command.EstaActivo;

        await _usuarioRepository.ActualizarAsync(usuario, cancellationToken);

        return new CambiarEstadoEmpleadoResponse(usuario.Nombre, usuario.EstaActivo);
    }
}
