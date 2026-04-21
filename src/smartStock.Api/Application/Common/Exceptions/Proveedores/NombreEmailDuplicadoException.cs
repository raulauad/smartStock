using Microsoft.AspNetCore.Http;
using smartStock.Api.Application.Common.Exceptions;

namespace smartStock.Api.Application.Common.Exceptions.Proveedores;

public sealed class NombreEmailDuplicadoException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Nombre y email de proveedor ya registrados";

    public NombreEmailDuplicadoException()
        : base("Ya existe un proveedor con esa combinación de nombre y email. Verificá si se trata del mismo proveedor.") { }
}
