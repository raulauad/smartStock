using MediatR;
using smartStock.Api.Application.Common.Exceptions.Admin;
using smartStock.Api.Application.Common.Interfaces.Auth;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Enums;
using smartStock.Api.Domain.Models;
using smartStock.Shared.Dtos.Admin.RegistrarAdmin;

namespace smartStock.Api.Application.Features.Admin.Commands.RegistrarAdmin;

public sealed class RegistrarAdministradorCommandHandler
    : IRequestHandler<RegistrarAdministradorCommand, RegistrarAdministradorResponse>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher    _passwordHasher;

    public RegistrarAdministradorCommandHandler(
        IUsuarioRepository usuarioRepository,
        IPasswordHasher    passwordHasher)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher    = passwordHasher;
    }

    public async Task<RegistrarAdministradorResponse> Handle(
        RegistrarAdministradorCommand request,
        CancellationToken             cancellationToken)
    {
        // FA2: bloquear si ya existe un administrador registrado
        if (await _usuarioRepository.ExisteAdministradorAsync(cancellationToken))
            throw new AdminYaExisteException();

        var usuario = new Usuario
        {
            Email          = request.Email,
            ContrasenaHash = _passwordHasher.Hashear(request.Contrasena),
            Nombre         = request.Nombre,
            Telefono       = request.Telefono,
            Dni            = request.Dni,
            Direccion      = new Direccion
            {
                Pais         = request.Direccion.Pais,
                Provincia    = request.Direccion.Provincia,
                Localidad    = request.Direccion.Localidad,
                CodigoPostal = request.Direccion.CodigoPostal,
                Calle        = request.Direccion.Calle,
                Numero       = request.Direccion.Numero
            },
            FechaAlta  = DateTime.UtcNow,
            EstaActivo = true,
            Roles      = [new UsuarioRol { Rol = Rol.Administrador }]
        };

        await _usuarioRepository.CrearAsync(usuario, cancellationToken);

        return new RegistrarAdministradorResponse(usuario.Id, usuario.Nombre, usuario.Email);
    }
}
