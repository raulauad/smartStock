using Microsoft.AspNetCore.Http;

namespace smartStock.Api.Application.Common.Exceptions.Productos;

public sealed class ProductoNoEncontradoException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status404NotFound;
    public string Titulo     => "Producto no encontrado";

    public ProductoNoEncontradoException()
        : base("No se encontró el producto solicitado.") { }
}
