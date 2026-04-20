using Microsoft.EntityFrameworkCore;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Repositories;

public sealed class TokenRevocadoRepository : ITokenRevocadoRepository
{
    private readonly AppDbContext _db;

    public TokenRevocadoRepository(AppDbContext db) => _db = db;

    public async Task RevocarAsync(string jti, DateTime expiracion, Guid usuarioId, CancellationToken ct = default)
    {
        _db.TokensRevocados.Add(new TokenRevocado
        {
            Jti        = jti,
            Expiracion = expiracion,
            UsuarioId  = usuarioId
        });
        await _db.SaveChangesAsync(ct);
    }

    public Task<bool> EstaRevocadoAsync(string jti, CancellationToken ct = default)
        => _db.TokensRevocados.AnyAsync(t => t.Jti == jti, ct);
}
