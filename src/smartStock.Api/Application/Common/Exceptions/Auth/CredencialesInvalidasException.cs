using Microsoft.AspNetCore.Http;
using smartStock.Api.Application.Common.Exceptions;

namespace smartStock.Api.Application.Common.Exceptions.Auth;

public sealed class CredencialesInvalidasException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status401Unauthorized;
    public string Titulo     => "Credenciales inválidas";

    public CredencialesInvalidasException()
        : base("El email o la contraseña son incorrectos.") { }
}
