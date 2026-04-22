using Microsoft.AspNetCore.Http;
using smartStock.Api.Application.Common.Exceptions;

namespace smartStock.Api.Application.Common.Exceptions.Categorias;

public sealed class CategoriaReasignacionRequiereDestinoException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status422UnprocessableEntity;
    public string Titulo     => "Reasignación de productos requerida";

    public CategoriaReasignacionRequiereDestinoException()
        : base("La categoría tiene productos asociados. Debe indicar una categoría destino para reasignarlos antes de desactivar.") { }
}
