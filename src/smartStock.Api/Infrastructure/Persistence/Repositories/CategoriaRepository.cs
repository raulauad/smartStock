using Microsoft.EntityFrameworkCore;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Repositories;

public sealed class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _db;

    public CategoriaRepository(AppDbContext db) => _db = db;

    public Task<Categoria?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default)
        => _db.Categorias.FirstOrDefaultAsync(c => c.Id == id, ct);

    public Task<bool> NombreExisteAsync(string nombre, Guid? excluirId, CancellationToken ct = default)
        => _db.Categorias.AnyAsync(
               c => c.Nombre.ToLower() == nombre.ToLower()
                    && (excluirId == null || c.Id != excluirId), ct);

    public Task<bool> TieneProductosAsync(Guid categoriaId, CancellationToken ct = default)
        => _db.Productos.AnyAsync(p => p.CategoriaId == categoriaId, ct);

    public Task<bool> ExistenActivasAlternativasAsync(Guid excluirId, CancellationToken ct = default)
        => _db.Categorias.AnyAsync(c => c.EstaActivo && c.Id != excluirId, ct);

    public Task<int> ReasignarProductosAsync(Guid desdeId, Guid hastaId, CancellationToken ct = default)
        => _db.Productos
              .Where(p => p.CategoriaId == desdeId)
              .ExecuteUpdateAsync(s => s.SetProperty(p => p.CategoriaId, hastaId), ct);

    public async Task CrearAsync(Categoria categoria, CancellationToken ct = default)
    {
        _db.Categorias.Add(categoria);
        await _db.SaveChangesAsync(ct);
    }

    public async Task ActualizarAsync(Categoria categoria, CancellationToken ct = default)
    {
        await _db.SaveChangesAsync(ct);
    }
}
