using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Domain.Enums;
using smartStock.Api.Infrastructure.Persistence;
using smartStock.Shared.Dtos.Admin.ObtenerListaEmpleados;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Queries.ObtenerListaEmpleados;

public sealed class ObtenerListaEmpleadosQueryHandler
    : IRequestHandler<ObtenerListaEmpleadosQuery, IReadOnlyList<ObtenerListaEmpleadosResponse>>
{
    private readonly AppDbContext _db;

    public ObtenerListaEmpleadosQueryHandler(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<ObtenerListaEmpleadosResponse>> Handle(
        ObtenerListaEmpleadosQuery query,
        CancellationToken          cancellationToken)
        => await _db.Usuarios
               .Where(u => u.Roles.Any(r => r.Rol == Rol.Empleado))
               .Select(u => new ObtenerListaEmpleadosResponse(
                   u.Id,
                   u.Nombre,
                   u.Email,
                   u.EstaActivo))
               .ToListAsync(cancellationToken);
}
