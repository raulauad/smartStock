using Microsoft.AspNetCore.Http;
using smartStock.Api.Application.Common.Exceptions;

namespace smartStock.Api.Application.Common.Exceptions.Proveedores;

public sealed class EstadoProveedorSinCambioException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Estado del proveedor sin cambio";

    public EstadoProveedorSinCambioException(bool estaActivo)
        : base(estaActivo
            ? "El proveedor ya se encuentra activo."
            : "El proveedor ya se encuentra inactivo.") { }
}
