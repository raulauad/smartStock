using MediatR;
using smartStock.Api.Application.Common.Exceptions.Auth;
using smartStock.Api.Application.Common.Exceptions.Usuarios;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Enums;

namespace smartStock.Api.Application.Features.Admin.Commands.EliminarEmpleado;

public sealed class EliminarEmpleadoCommandHandler : IRequestHandler<EliminarEmpleadoCommand>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public EliminarEmpleadoCommandHandler(IUsuarioRepository usuarioRepository)
        => _usuarioRepository = usuarioRepository;

    public async Task Handle(EliminarEmpleadoCommand command, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.ObtenerPorIdAsync(command.EmpleadoId, cancellationToken)
            ?? throw new UsuarioNoEncontradoException();

        // Solo se permite eliminar empleados, no administradores
        if (!usuario.Roles.Any(r => r.Rol == Rol.Empleado))
            throw new AccesoNoPermitidoException();

        await _usuarioRepository.EliminarAsync(usuario, cancellationToken);
    }
}
