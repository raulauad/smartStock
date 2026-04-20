using Microsoft.AspNetCore.Http;
using smartStock.Api.Application.Common.Exceptions;

namespace smartStock.Api.Application.Common.Exceptions.Auth;

public sealed class CuentaInactivaException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status403Forbidden;
    public string Titulo     => "Cuenta inactiva";

    public CuentaInactivaException()
        : base("La cuenta no está activa. Contacte al administrador.") { }
}
