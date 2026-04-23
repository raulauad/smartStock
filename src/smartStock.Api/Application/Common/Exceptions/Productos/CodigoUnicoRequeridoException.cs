using Microsoft.AspNetCore.Http;

namespace smartStock.Api.Application.Common.Exceptions.Productos;

public sealed class CodigoUnicoRequeridoException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Código único del producto";

    public CodigoUnicoRequeridoException()
        : base("Todo producto debe conservar al menos un código. " +
               "Agregue un nuevo código antes de eliminar el existente.") { }
}
