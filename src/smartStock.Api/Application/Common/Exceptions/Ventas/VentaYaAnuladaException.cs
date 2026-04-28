using Microsoft.AspNetCore.Http;

namespace smartStock.Api.Application.Common.Exceptions.Ventas;

public sealed class VentaYaAnuladaException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Venta ya anulada";

    public VentaYaAnuladaException()
        : base("La venta ya fue anulada previamente.") { }
}
