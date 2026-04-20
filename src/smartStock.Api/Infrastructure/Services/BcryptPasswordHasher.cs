using smartStock.Api.Application.Common.Interfaces.Auth;

namespace smartStock.Api.Infrastructure.Services;

public sealed class BcryptPasswordHasher : IPasswordHasher
{
    public string Hashear(string contrasena)
        => BCrypt.Net.BCrypt.HashPassword(contrasena);

    public bool Verificar(string contrasena, string hash)
        => BCrypt.Net.BCrypt.Verify(contrasena, hash);
}
