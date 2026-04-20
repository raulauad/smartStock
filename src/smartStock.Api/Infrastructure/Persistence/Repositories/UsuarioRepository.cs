using Microsoft.EntityFrameworkCore;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Enums;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Persistence.Repositories;

public sealed class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _db;

    public UsuarioRepository(AppDbContext db) => _db = db;

    public Task<Usuario?> ObtenerPorEmailAsync(string email, CancellationToken ct = default)
        => _db.Usuarios
              .Include(u => u.Roles)
              .FirstOrDefaultAsync(u => u.Email == email, ct);

    public Task<Usuario?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default)
        => _db.Usuarios
              .Include(u => u.Roles)
              .FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<bool> EmailExisteAsync(string email, Guid? excluirId, CancellationToken ct = default)
        => _db.Usuarios.AnyAsync(
               u => u.Email == email && (excluirId == null || u.Id != excluirId), ct);

    public Task<bool> DniExisteAsync(string dni, Guid? excluirId, CancellationToken ct = default)
        => _db.Usuarios.AnyAsync(
               u => u.Dni == dni && (excluirId == null || u.Id != excluirId), ct);

    public Task<bool> ExisteAdministradorAsync(CancellationToken ct = default)
        => _db.UsuarioRoles.AnyAsync(ur => ur.Rol == Rol.Administrador, ct);

    public Task<bool> EstaActivoAsync(Guid id, CancellationToken ct = default)
        => _db.Usuarios.AnyAsync(u => u.Id == id && u.EstaActivo, ct);

    public async Task CrearAsync(Usuario usuario, CancellationToken ct = default)
    {
        usuario.Id = Guid.NewGuid();
        _db.Usuarios.Add(usuario);
        await _db.SaveChangesAsync(ct);
    }

    public async Task ActualizarAsync(Usuario usuario, CancellationToken ct = default)
    {
        _db.Usuarios.Update(usuario);
        await _db.SaveChangesAsync(ct);
    }

    public async Task EliminarAsync(Usuario usuario, CancellationToken ct = default)
    {
        _db.Usuarios.Remove(usuario);
        await _db.SaveChangesAsync(ct);
    }
}
