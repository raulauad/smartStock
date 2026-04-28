using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Application.Common.Exceptions.Proveedores;
using smartStock.Api.Infrastructure.Persistence;
using smartStock.Shared.Dtos.Admin.ObtenerDetalleProveedor;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Queries.ObtenerDetalleProveedor;

public sealed class ObtenerDetalleProveedorQueryHandler
    : IRequestHandler<ObtenerDetalleProveedorQuery, ObtenerDetalleProveedorResponse>
{
    private readonly AppDbContext _db;

    public ObtenerDetalleProveedorQueryHandler(AppDbContext db) => _db = db;

    public async Task<ObtenerDetalleProveedorResponse> Handle(
        ObtenerDetalleProveedorQuery query,
        CancellationToken            cancellationToken)
    {
        return await _db.Proveedores
            .Where(p => p.Id == query.ProveedorId)
            .Select(p => new ObtenerDetalleProveedorResponse(
                p.Id,
                p.Nombre,
                p.Cuit,
                p.Telefono,
                p.Email,
                new DireccionProveedorResponse(
                    p.Direccion.Pais,
                    p.Direccion.Provincia,
                    p.Direccion.Localidad,
                    p.Direccion.CodigoPostal,
                    p.Direccion.Calle,
                    p.Direccion.Numero),
                p.Observaciones,
                p.EstaActivo,
                p.FechaAlta,
                p.UsuarioAlta.Nombre))
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new ProveedorNoEncontradoException();
    }
}
