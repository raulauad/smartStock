using Microsoft.AspNetCore.Http;
using smartStock.Api.Application.Common.Exceptions;

namespace smartStock.Api.Application.Common.Exceptions.Categorias;

public sealed class SinCategoriasActivasAlternativasException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Sin categorías activas alternativas";

    public SinCategoriasActivasAlternativasException()
        : base("No se puede desactivar la categoría porque no existen otras categorías activas a las que reasignar los productos.") { }
}
