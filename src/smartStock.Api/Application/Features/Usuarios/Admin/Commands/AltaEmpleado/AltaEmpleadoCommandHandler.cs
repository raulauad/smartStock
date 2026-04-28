using MediatR;
using smartStock.Api.Application.Common.Exceptions.Usuarios;
using smartStock.Api.Application.Common.Interfaces.Auth;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Enums;
using smartStock.Api.Domain.Models;
using smartStock.Shared.Dtos.Admin.AltaEmpleado;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Commands.AltaEmpleado;

public sealed class AltaEmpleadoCommandHandler
    : IRequestHandler<AltaEmpleadoCommand, AltaEmpleadoResponse>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher    _passwordHasher;

    public AltaEmpleadoCommandHandler(
        IUsuarioRepository usuarioRepository,
        IPasswordHasher    passwordHasher)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher    = passwordHasher;
    }

    public async Task<AltaEmpleadoResponse> Handle(
        AltaEmpleadoCommand command,
        CancellationToken   cancellationToken)
    {
        // FA2: DNI duplicado
        if (await _usuarioRepository.DniExisteAsync(command.Dni, null, cancellationToken))
            throw new DniDuplicadoException();

        // FA2: Email duplicado
        if (await _usuarioRepository.EmailExisteAsync(command.Email, null, cancellationToken))
            throw new EmailDuplicadoException();

        var usuario = new Usuario
        {
            Email          = command.Email,
            ContrasenaHash = _passwordHasher.Hashear(command.Contrasena),
            Nombre         = command.Nombre,
            Telefono       = command.Telefono,
            Dni            = command.Dni,
            Direccion      = new Direccion
            {
                Pais         = command.Direccion.Pais,
                Provincia    = command.Direccion.Provincia,
                Localidad    = command.Direccion.Localidad,
                CodigoPostal = command.Direccion.CodigoPostal,
                Calle        = command.Direccion.Calle,
                Numero       = command.Direccion.Numero
            },
            FechaAlta  = DateTime.UtcNow,
            EstaActivo = true,
            Roles      = [new UsuarioRol { Rol = Rol.Empleado }]
        };

        await _usuarioRepository.CrearAsync(usuario, cancellationToken);

        return new AltaEmpleadoResponse(usuario.Id, usuario.Nombre, usuario.Email);
    }
}
