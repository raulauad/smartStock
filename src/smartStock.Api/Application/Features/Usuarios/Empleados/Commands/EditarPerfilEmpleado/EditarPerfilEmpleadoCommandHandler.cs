using MediatR;
using smartStock.Api.Application.Common.Exceptions.Usuarios;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Models;
using smartStock.Shared.Dtos.Empleados.EditarPerfilEmpleado;

namespace smartStock.Api.Application.Features.Usuarios.Empleados.Commands.EditarPerfilEmpleado;

public sealed class EditarPerfilEmpleadoCommandHandler
    : IRequestHandler<EditarPerfilEmpleadoCommand, EditarPerfilEmpleadoResponse>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public EditarPerfilEmpleadoCommandHandler(IUsuarioRepository usuarioRepository)
        => _usuarioRepository = usuarioRepository;

    public async Task<EditarPerfilEmpleadoResponse> Handle(
        EditarPerfilEmpleadoCommand command,
        CancellationToken           cancellationToken)
    {
        var usuario = await _usuarioRepository.ObtenerPorIdAsync(command.UsuarioId, cancellationToken)
            ?? throw new UsuarioNoEncontradoException();

        // FA1: DNI duplicado en otro usuario
        if (await _usuarioRepository.DniExisteAsync(command.Dni, command.UsuarioId, cancellationToken))
            throw new DniDuplicadoException();

        // FA1: Email duplicado en otro usuario
        if (await _usuarioRepository.EmailExisteAsync(command.Email, command.UsuarioId, cancellationToken))
            throw new EmailDuplicadoException();

        usuario.Nombre   = command.Nombre;
        usuario.Telefono = command.Telefono;
        usuario.Dni      = command.Dni;
        usuario.Email    = command.Email;
        usuario.Direccion = new Direccion
        {
            Pais         = command.Direccion.Pais,
            Provincia    = command.Direccion.Provincia,
            Localidad    = command.Direccion.Localidad,
            CodigoPostal = command.Direccion.CodigoPostal,
            Calle        = command.Direccion.Calle,
            Numero       = command.Direccion.Numero
        };

        await _usuarioRepository.ActualizarAsync(usuario, cancellationToken);

        return new EditarPerfilEmpleadoResponse(
            usuario.Nombre,
            usuario.Email,
            usuario.Telefono,
            usuario.Dni);
    }
}
