using Microsoft.AspNetCore.Http;
using smartStock.Api.Application.Common.Exceptions;

namespace smartStock.Api.Application.Common.Exceptions.Categorias;

public sealed class CategoriaNoEncontradaException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status404NotFound;
    public string Titulo     => "Categoría no encontrada";

    public CategoriaNoEncontradaException()
        : base("No se encontró la categoría solicitada.") { }
}
