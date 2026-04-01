using smartStock.Domain.Models;

namespace smartStock.Application.Common.Interfaces;

public interface IJwtTokenService
{
    (string Token, DateTime Expiracion) GenerarToken(Usuario usuario, IList<string> roles);
}
