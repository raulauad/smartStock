using MediatR;
using Microsoft.AspNetCore.Identity;
using smartStock.Application.Common.Exceptions.Usuarios;
using smartStock.Domain.Models;

namespace smartStock.Application.Features.Queries.Usuarios.ObtenerPerfilAdmin;

public sealed class ObtenerPerfilAdminQueryHandler
    : IRequestHandler<ObtenerPerfilAdminQuery, ObtenerPerfilAdminResponse>
{
    private readonly UserManager<Usuario> _userManager;

    public ObtenerPerfilAdminQueryHandler(UserManager<Usuario> userManager)
        => _userManager = userManager;

    public async Task<ObtenerPerfilAdminResponse> Handle(
        ObtenerPerfilAdminQuery query,
        CancellationToken       cancellationToken)
    {
        var admin = await _userManager.FindByIdAsync(query.AdminId.ToString())
            ?? throw new UsuarioNoEncontradoException();

        return new ObtenerPerfilAdminResponse(
            admin.Id,
            admin.Nombre,
            admin.Email!,
            admin.Telefono,
            admin.Dni);
    }
}
