using Microsoft.AspNetCore.Http;

namespace smartStock.Api.Application.Common.Exceptions.Ventas;

public sealed class VentaNoEncontradaException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status404NotFound;
    public string Titulo     => "Venta no encontrada";

    public VentaNoEncontradaException()
        : base("No se encontró la venta solicitada.") { }
}
