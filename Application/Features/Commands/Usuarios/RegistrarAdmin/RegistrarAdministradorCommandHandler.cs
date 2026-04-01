using MediatR;
using Microsoft.AspNetCore.Identity;
using smartStock.Application.Common.Exceptions;
using smartStock.Application.Common.Exceptions.Admin;
using smartStock.Application.Features.Commands.Usuarios.DTOs;
using smartStock.Domain.Models;

namespace smartStock.Application.Features.Commands.Usuarios.RegistrarAdmin;

public sealed class RegistrarAdministradorCommandHandler
    : IRequestHandler<RegistrarAdministradorCommand, RegistrarAdministradorResponse>
{
    private const string RolAdministrador = "Administrador";

    private readonly UserManager<Usuario>            _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public RegistrarAdministradorCommandHandler(
        UserManager<Usuario>            userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<RegistrarAdministradorResponse> Handle(
        RegistrarAdministradorCommand request,
        CancellationToken             cancellationToken)
    {
        // FA2: bloquear si ya existe un administrador registrado
        if (await _roleManager.RoleExistsAsync(RolAdministrador))
        {
            var adminsExistentes = await _userManager.GetUsersInRoleAsync(RolAdministrador);
            if (adminsExistentes.Count > 0)
                throw new AdminYaExisteException();
        }

        var usuario = new Usuario
        {
            UserName   = request.Email,
            Email      = request.Email,
            Nombre     = request.Nombre,
            Telefono   = request.Telefono,
            Dni        = request.Dni,
            Direccion  = new Domain.Models.Direccion
            {
                Pais         = request.Direccion.Pais,
                Provincia    = request.Direccion.Provincia,
                Localidad    = request.Direccion.Localidad,
                CodigoPostal = request.Direccion.CodigoPostal,
                Calle        = request.Direccion.Calle,
                Numero       = request.Direccion.Numero
            },
            FechaAlta  = DateTime.UtcNow,
            EstaActivo = true
        };

        var resultado = await _userManager.CreateAsync(usuario, request.Contrasena);

        if (!resultado.Succeeded)
            throw new IdentityException(resultado.Errors);

        if (!await _roleManager.RoleExistsAsync(RolAdministrador))
            await _roleManager.CreateAsync(new IdentityRole<Guid>(RolAdministrador));

        await _userManager.AddToRoleAsync(usuario, RolAdministrador);

        return new RegistrarAdministradorResponse(usuario.Id, usuario.Nombre, usuario.Email!);
    }
}
