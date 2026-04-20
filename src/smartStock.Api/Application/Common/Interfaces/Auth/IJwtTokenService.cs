using smartStock.Api.Domain.Models;

namespace smartStock.Api.Application.Common.Interfaces.Auth;

public interface IJwtTokenService
{
    (string Token, DateTime Expiracion) GenerarToken(Usuario usuario, IList<string> roles);
}
