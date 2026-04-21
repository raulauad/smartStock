using Microsoft.AspNetCore.Http;
using smartStock.Api.Application.Common.Exceptions;

namespace smartStock.Api.Application.Common.Exceptions.Proveedores;

public sealed class CuitDuplicadoException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "CUIT ya registrado";

    public CuitDuplicadoException()
        : base("Ya existe un proveedor registrado con ese CUIT.") { }
}
