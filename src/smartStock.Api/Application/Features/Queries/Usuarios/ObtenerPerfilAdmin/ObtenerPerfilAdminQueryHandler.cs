using MediatR;
using Microsoft.AspNetCore.Identity;
using smartStock.Api.Application.Common.Exceptions.Auth;
using smartStock.Api.Application.Common.Exceptions.Usuarios;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Application.Features.Queries.Usuarios.ObtenerPerfilAdmin;

public sealed class ObtenerPerfilAdminQueryHandler
    : IRequestHandler<ObtenerPerfilAdminQuery, ObtenerPerfilAdminResponse>
{
    private const string RolAdministrador = "Administrador";

    private readonly UserManager<Usuario> _userManager;

    public ObtenerPerfilAdminQueryHandler(UserManager<Usuario> userManager)
        => _userManager = userManager;

    public async Task<ObtenerPerfilAdminResponse> Handle(
        ObtenerPerfilAdminQuery query,
        CancellationToken       cancellationToken)
    {
        var admin = await _userManager.FindByIdAsync(query.AdminId.ToString())
            ?? throw new UsuarioNoEncontradoException();

        if (!await _userManager.IsInRoleAsync(admin, RolAdministrador))
            throw new AccesoNoPermitidoException();

        return new ObtenerPerfilAdminResponse(
            admin.Id,
            admin.Nombre,
            admin.Email!,
            admin.Telefono,
            admin.Dni
        );
    }
}
