using Microsoft.AspNetCore.Http;
using smartStock.Api.Application.Common.Exceptions;

namespace smartStock.Api.Application.Common.Exceptions.Categorias;

public sealed class CategoriaNombreDuplicadoException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Nombre de categoría ya registrado";

    public CategoriaNombreDuplicadoException()
        : base("Ya existe una categoría con ese nombre.") { }
}
