using Microsoft.EntityFrameworkCore;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Repositories;

public sealed class ProductoRepository : IProductoRepository
{
    private readonly AppDbContext _db;

    public ProductoRepository(AppDbContext db) => _db = db;

    public Task<Producto?> ObtenerPorIdAsync(Guid productoId, CancellationToken ct = default)
        => _db.Productos
              .Include(p => p.Codigos)
              .Include(p => p.Stock)
              .FirstOrDefaultAsync(p => p.Id == productoId, ct);

    public Task<bool> CodigoExisteAsync(string codigo, Guid? excluirProductoId, CancellationToken ct = default)
        => _db.CodigosProducto.AnyAsync(
               c => c.Codigo == codigo && (excluirProductoId == null || c.ProductoId != excluirProductoId), ct);

    public async Task<List<string>> ObtenerNombresSimilaresAsync(string nombre, Guid? excluirId, CancellationToken ct = default)
    {
        var nombreLower = nombre.ToLower();
        return await _db.Productos
            .Where(p => p.EstaActivo
                     && (excluirId == null || p.Id != excluirId)
                     && p.Nombre.ToLower() == nombreLower)
            .Select(p => p.Nombre)
            .ToListAsync(ct);
    }

    public async Task<string> GenerarCodigoInternoAsync(CancellationToken ct = default)
    {
        var codigos = await _db.CodigosProducto
            .Where(c => c.Codigo.StartsWith("PROD-"))
            .Select(c => c.Codigo)
            .ToListAsync(ct);

        var maxNumero = codigos
            .Select(c => int.TryParse(c.AsSpan(5), out var n) ? n : 0)
            .DefaultIfEmpty(0)
            .Max();

        return $"PROD-{maxNumero + 1:D5}";
    }

    public Task<CodigoProducto?> ObtenerCodigoPorIdAsync(Guid codigoId, Guid productoId, CancellationToken ct = default)
        => _db.CodigosProducto.FirstOrDefaultAsync(c => c.Id == codigoId && c.ProductoId == productoId, ct);

    public Task<int> ContarCodigosAsync(Guid productoId, CancellationToken ct = default)
        => _db.CodigosProducto.CountAsync(c => c.ProductoId == productoId, ct);

    public async Task CrearAsync(Producto producto, CancellationToken ct = default)
    {
        _db.Productos.Add(producto);
        await _db.SaveChangesAsync(ct);
    }

    public async Task ActualizarAsync(Producto producto, CancellationToken ct = default)
    {
        await _db.SaveChangesAsync(ct);
    }

    public async Task AgregarCodigoAsync(CodigoProducto codigo, CancellationToken ct = default)
    {
        _db.CodigosProducto.Add(codigo);
        await _db.SaveChangesAsync(ct);
    }

    public async Task ActualizarCodigoAsync(CodigoProducto codigo, CancellationToken ct = default)
    {
        await _db.SaveChangesAsync(ct);
    }

    public async Task EliminarCodigoAsync(CodigoProducto codigo, CancellationToken ct = default)
    {
        _db.CodigosProducto.Remove(codigo);
        await _db.SaveChangesAsync(ct);
    }

    public async Task AjustarStockAsync(MovimientoStock movimiento, CancellationToken ct = default)
    {
        _db.MovimientosStock.Add(movimiento);
        await _db.SaveChangesAsync(ct);
    }
}
