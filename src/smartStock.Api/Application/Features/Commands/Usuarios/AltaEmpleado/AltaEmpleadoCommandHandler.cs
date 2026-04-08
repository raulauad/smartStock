using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Application.Common.Exceptions;
using smartStock.Api.Application.Common.Exceptions.Usuarios;
using smartStock.Api.Application.Features.Commands.Usuarios.DTOs;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Application.Features.Commands.Usuarios.AltaEmpleado;

public sealed class AltaEmpleadoCommandHandler
    : IRequestHandler<AltaEmpleadoCommand, AltaEmpleadoResponse>
{
    private const string RolEmpleado = "Empleado";

    private readonly UserManager<Usuario>            _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public AltaEmpleadoCommandHandler(
        UserManager<Usuario>            userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<AltaEmpleadoResponse> Handle(
        AltaEmpleadoCommand command,
        CancellationToken   cancellationToken)
    {
        // FA2: DNI duplicado
        var dniExiste = await _userManager.Users
            .AnyAsync(u => u.Dni == command.Dni, cancellationToken);

        if (dniExiste)
            throw new DniDuplicadoException();

        var usuario = new Usuario
        {
            UserName   = command.Email,
            Email      = command.Email,
            Nombre     = command.Nombre,
            Telefono   = command.Telefono,
            Dni        = command.Dni,
            Direccion  = new Domain.Models.Direccion
            {
                Pais         = command.Direccion.Pais,
                Provincia    = command.Direccion.Provincia,
                Localidad    = command.Direccion.Localidad,
                CodigoPostal = command.Direccion.CodigoPostal,
                Calle        = command.Direccion.Calle,
                Numero       = command.Direccion.Numero
            },
            FechaAlta  = DateTime.UtcNow,
            EstaActivo = true
        };

        var resultado = await _userManager.CreateAsync(usuario, command.Contrasena);

        if (!resultado.Succeeded)
        {
            // FA2: Email duplicado detectado por Identity
            if (resultado.Errors.Any(e => e.Code is "DuplicateEmail" or "DuplicateUserName"))
                throw new EmailDuplicadoException();

            throw new IdentityException(resultado.Errors);
        }

        if (!await _roleManager.RoleExistsAsync(RolEmpleado))
            await _roleManager.CreateAsync(new IdentityRole<Guid>(RolEmpleado));

        await _userManager.AddToRoleAsync(usuario, RolEmpleado);

        return new AltaEmpleadoResponse(usuario.Id, usuario.Nombre, usuario.Email!);
    }
}
