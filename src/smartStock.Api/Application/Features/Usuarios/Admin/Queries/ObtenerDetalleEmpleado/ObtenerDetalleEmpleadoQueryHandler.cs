using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Application.Common.Exceptions.Usuarios;
using smartStock.Api.Domain.Enums;
using smartStock.Api.Infrastructure.Persistence;
using smartStock.Shared.Dtos.Admin.ObtenerDetalleEmpleado;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Queries.ObtenerDetalleEmpleado;

public sealed class ObtenerDetalleEmpleadoQueryHandler
    : IRequestHandler<ObtenerDetalleEmpleadoQuery, ObtenerDetalleEmpleadoResponse>
{
    private readonly AppDbContext _db;

    public ObtenerDetalleEmpleadoQueryHandler(AppDbContext db) => _db = db;

    public async Task<ObtenerDetalleEmpleadoResponse> Handle(
        ObtenerDetalleEmpleadoQuery query,
        CancellationToken           cancellationToken)
    {
        var empleado = await _db.Usuarios
            .Where(u => u.Id == query.EmpleadoId && u.Roles.Any(r => r.Rol == Rol.Empleado))
            .Select(u => new ObtenerDetalleEmpleadoResponse(
                u.Id,
                u.Nombre,
                u.Email,
                u.Telefono,
                u.Dni,
                new DireccionEmpleadoResponse(
                    u.Direccion.Pais,
                    u.Direccion.Provincia,
                    u.Direccion.Localidad,
                    u.Direccion.CodigoPostal,
                    u.Direccion.Calle,
                    u.Direccion.Numero)))
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new UsuarioNoEncontradoException();

        return empleado;
    }
}
