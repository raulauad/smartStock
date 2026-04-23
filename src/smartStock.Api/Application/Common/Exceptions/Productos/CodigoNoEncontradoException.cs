using Microsoft.AspNetCore.Http;

namespace smartStock.Api.Application.Common.Exceptions.Productos;

public sealed class CodigoNoEncontradoException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status404NotFound;
    public string Titulo     => "Código de producto no encontrado";

    public CodigoNoEncontradoException()
        : base("No se encontró el código solicitado para este producto.") { }
}
