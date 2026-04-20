using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Application.Common.Exceptions.Usuarios;
using smartStock.Api.Domain.Enums;
using smartStock.Api.Infrastructure.Persistence;
using smartStock.Shared.Dtos.Admin.ObtenerPerfilAdmin;

namespace smartStock.Api.Application.Features.Admin.Queries.ObtenerPerfilAdmin;

public sealed class ObtenerPerfilAdminQueryHandler
    : IRequestHandler<ObtenerPerfilAdminQuery, ObtenerPerfilAdminResponse>
{
    private readonly AppDbContext _db;

    public ObtenerPerfilAdminQueryHandler(AppDbContext db) => _db = db;

    public async Task<ObtenerPerfilAdminResponse> Handle(
        ObtenerPerfilAdminQuery query,
        CancellationToken       cancellationToken)
        => await _db.Usuarios
               .Where(u => u.Id == query.AdminId && u.Roles.Any(r => r.Rol == Rol.Administrador))
               .Select(u => new ObtenerPerfilAdminResponse(
                   u.Id,
                   u.Nombre,
                   u.Email,
                   u.Telefono,
                   u.Dni))
               .FirstOrDefaultAsync(cancellationToken)
           ?? throw new UsuarioNoEncontradoException();
}
