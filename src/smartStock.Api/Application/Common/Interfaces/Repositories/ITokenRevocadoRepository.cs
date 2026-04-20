namespace smartStock.Api.Application.Common.Interfaces.Repositories;

public interface ITokenRevocadoRepository
{
    Task       RevocarAsync       (string jti, DateTime expiracion, Guid usuarioId, CancellationToken ct = default);
    Task<bool> EstaRevocadoAsync  (string jti, CancellationToken ct = default);
}
