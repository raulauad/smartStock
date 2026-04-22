using Microsoft.AspNetCore.Http;
using smartStock.Api.Application.Common.Exceptions;

namespace smartStock.Api.Application.Common.Exceptions.Categorias;

public sealed class EstadoCategoriaSinCambioException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Estado de categoría sin cambio";

    public EstadoCategoriaSinCambioException(bool estaActivo)
        : base(estaActivo
            ? "La categoría ya se encuentra activa."
            : "La categoría ya se encuentra inactiva.") { }
}
