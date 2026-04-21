using Microsoft.AspNetCore.Http;
using smartStock.Api.Application.Common.Exceptions;

namespace smartStock.Api.Application.Common.Exceptions.Proveedores;

public sealed class ProveedorNoEncontradoException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status404NotFound;
    public string Titulo     => "Proveedor no encontrado";

    public ProveedorNoEncontradoException()
        : base("No se encontró el proveedor solicitado.") { }
}
