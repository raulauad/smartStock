using Microsoft.AspNetCore.Http;
using smartStock.Api.Application.Common.Exceptions;

namespace smartStock.Api.Application.Common.Exceptions.Proveedores;

public sealed class EmailProveedorDuplicadoException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Email de proveedor ya registrado";

    public EmailProveedorDuplicadoException()
        : base("Ya existe un proveedor con ese email.") { }
}
