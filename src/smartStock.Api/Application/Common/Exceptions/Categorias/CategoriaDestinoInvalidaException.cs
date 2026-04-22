using Microsoft.AspNetCore.Http;
using smartStock.Api.Application.Common.Exceptions;

namespace smartStock.Api.Application.Common.Exceptions.Categorias;

public sealed class CategoriaDestinoInvalidaException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status422UnprocessableEntity;
    public string Titulo     => "Categoría destino inválida";

    public CategoriaDestinoInvalidaException()
        : base("La categoría destino no existe, está inactiva o es la misma que se desea desactivar.") { }
}
