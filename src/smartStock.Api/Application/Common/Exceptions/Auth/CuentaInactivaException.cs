using Microsoft.AspNetCore.Http;
using smartStock.Api.Application.Common.Interfaces;

namespace smartStock.Api.Application.Common.Exceptions.Auth;

public sealed class CuentaInactivaException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status401Unauthorized;
    public string Titulo     => "Cuenta inactiva";

    public CuentaInactivaException()
        : base("La cuenta no está activa. Contacte al administrador.") { }
}
