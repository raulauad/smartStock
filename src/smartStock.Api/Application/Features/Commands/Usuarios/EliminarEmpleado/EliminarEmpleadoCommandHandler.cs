using MediatR;
using Microsoft.AspNetCore.Identity;
using smartStock.Api.Application.Common.Exceptions;
using smartStock.Api.Application.Common.Exceptions.Auth;
using smartStock.Api.Application.Common.Exceptions.Usuarios;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Application.Features.Commands.Usuarios.EliminarEmpleado;

public sealed class EliminarEmpleadoCommandHandler : IRequestHandler<EliminarEmpleadoCommand>
{
    private const string RolEmpleado = "Empleado";

    private readonly UserManager<Usuario> _userManager;

    public EliminarEmpleadoCommandHandler(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(EliminarEmpleadoCommand command, CancellationToken cancellationToken)
    {
        var usuario = await _userManager.FindByIdAsync(command.EmpleadoId.ToString());

        if (usuario is null)
            throw new UsuarioNoEncontradoException();

        // Solo se permite eliminar empleados, no administradores
        if (!await _userManager.IsInRoleAsync(usuario, RolEmpleado))
            throw new AccesoNoPermitidoException();

        var resultado = await _userManager.DeleteAsync(usuario);

        if (!resultado.Succeeded)
            throw new IdentityException(resultado.Errors);
    }
}
