using Microsoft.AspNetCore.Http;

namespace smartStock.Api.Application.Common.Exceptions.Compras;

public sealed class CompraNoEncontradaException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status404NotFound;
    public string Titulo     => "Compra no encontrada";

    public CompraNoEncontradaException()
        : base("No se encontró la compra solicitada.") { }
}
