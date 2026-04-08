using Microsoft.AspNetCore.Http;
using smartStock.Api.Application.Common.Interfaces;

namespace smartStock.Api.Application.Common.Exceptions.Admin;

public sealed class AdminYaExisteException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Administrador ya registrado";

    public AdminYaExisteException()
        : base("Ya existe un administrador registrado en el sistema. Esta operación solo puede realizarse una vez.") { }
}
