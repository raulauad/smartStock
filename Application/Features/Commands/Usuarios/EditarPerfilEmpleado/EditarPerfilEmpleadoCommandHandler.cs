using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using smartStock.Application.Common.Exceptions;
using smartStock.Application.Common.Exceptions.Usuarios;
using smartStock.Domain.Models;

namespace smartStock.Application.Features.Commands.Usuarios.EditarPerfilEmpleado;

public sealed class EditarPerfilEmpleadoCommandHandler
    : IRequestHandler<EditarPerfilEmpleadoCommand, EditarPerfilEmpleadoResponse>
{
    private readonly UserManager<Usuario> _userManager;

    public EditarPerfilEmpleadoCommandHandler(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }

    public async Task<EditarPerfilEmpleadoResponse> Handle(
        EditarPerfilEmpleadoCommand command,
        CancellationToken           cancellationToken)
    {
        var usuario = await _userManager.FindByIdAsync(command.UsuarioId.ToString());

        if (usuario is null)
            throw new UsuarioNoEncontradoException();

        // FA1: DNI duplicado en otro usuario
        var dniEnUso = await _userManager.Users
            .AnyAsync(u => u.Dni == command.Dni && u.Id != command.UsuarioId, cancellationToken);

        if (dniEnUso)
            throw new DniDuplicadoException();

        // FA1: Email duplicado en otro usuario
        var emailEnUso = await _userManager.Users
            .AnyAsync(u => u.Email == command.Email && u.Id != command.UsuarioId, cancellationToken);

        if (emailEnUso)
            throw new EmailDuplicadoException();

        usuario.Nombre   = command.Nombre;
        usuario.Telefono = command.Telefono;
        usuario.Dni      = command.Dni;
        usuario.Direccion = new Direccion
        {
            Pais         = command.Direccion.Pais,
            Provincia    = command.Direccion.Provincia,
            Localidad    = command.Direccion.Localidad,
            CodigoPostal = command.Direccion.CodigoPostal,
            Calle        = command.Direccion.Calle,
            Numero       = command.Direccion.Numero
        };

        if (usuario.Email != command.Email)
        {
            usuario.Email    = command.Email;
            usuario.UserName = command.Email;
        }

        var resultado = await _userManager.UpdateAsync(usuario);

        if (!resultado.Succeeded)
        {
            if (resultado.Errors.Any(e => e.Code is "DuplicateEmail" or "DuplicateUserName"))
                throw new EmailDuplicadoException();

            throw new IdentityException(resultado.Errors);
        }

        return new EditarPerfilEmpleadoResponse(
            usuario.Nombre,
            usuario.Email!,
            usuario.Telefono,
            usuario.Dni
        );
    }
}
