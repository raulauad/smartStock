namespace smartStock.Api.Application.Common.Interfaces.Auth;

public interface IPasswordHasher
{
    string Hashear(string contrasena);
    bool   Verificar(string contrasena, string hash);
}
