using Microsoft.AspNetCore.Http;
using smartStock.Api.Application.Common.Exceptions;

namespace smartStock.Api.Application.Common.Exceptions.Proveedores;

public sealed class NombreTelefonoDuplicadoException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Nombre y teléfono de proveedor ya registrados";

    public NombreTelefonoDuplicadoException()
        : base("Ya existe un proveedor con esa combinación de nombre y teléfono. Verificá si se trata del mismo proveedor.") { }
}
