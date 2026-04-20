using Microsoft.AspNetCore.Http;
using smartStock.Api.Application.Common.Exceptions;

namespace smartStock.Api.Application.Common.Exceptions.Auth;

public sealed class AccesoNoPermitidoException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status403Forbidden;
    public string Titulo     => "Acceso no permitido";

    public AccesoNoPermitidoException()
        : base("No tiene permisos para realizar esta acción.") { }
}
