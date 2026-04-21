using Microsoft.EntityFrameworkCore;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Repositories;

public sealed class ProveedorRepository : IProveedorRepository
{
    private readonly AppDbContext _db;

    public ProveedorRepository(AppDbContext db) => _db = db;

    public Task<Proveedor?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default)
        => _db.Proveedores
              .FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<bool> CuitExisteAsync(string cuit, Guid? excluirId, CancellationToken ct = default)
        => _db.Proveedores.AnyAsync(
               p => p.Cuit == cuit && (excluirId == null || p.Id != excluirId), ct);

    public Task<bool> NombreEmailExisteAsync(string nombre, string email, Guid? excluirId, CancellationToken ct = default)
        => _db.Proveedores.AnyAsync(
               p => p.Nombre == nombre && p.Email == email && (excluirId == null || p.Id != excluirId), ct);

    public Task<bool> NombreTelefonoExisteAsync(string nombre, string telefono, Guid? excluirId, CancellationToken ct = default)
        => _db.Proveedores.AnyAsync(
               p => p.Nombre == nombre && p.Telefono == telefono && (excluirId == null || p.Id != excluirId), ct);

    public async Task CrearAsync(Proveedor proveedor, CancellationToken ct = default)
    {
        proveedor.Id = Guid.NewGuid();
        _db.Proveedores.Add(proveedor);
        await _db.SaveChangesAsync(ct);
    }

    public async Task ActualizarAsync(Proveedor proveedor, CancellationToken ct = default)
    {
        _db.Proveedores.Update(proveedor);
        await _db.SaveChangesAsync(ct);
    }
}
