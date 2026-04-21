using Microsoft.AspNetCore.Http;
using smartStock.Api.Application.Common.Exceptions;

namespace smartStock.Api.Application.Common.Exceptions.Proveedores;

public sealed class TelefonoDuplicadoException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Teléfono de proveedor ya registrado";

    public TelefonoDuplicadoException()
        : base("Ya existe un proveedor con ese teléfono.") { }
}
