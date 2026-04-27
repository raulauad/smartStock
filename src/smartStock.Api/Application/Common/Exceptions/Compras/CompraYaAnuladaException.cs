using Microsoft.AspNetCore.Http;

namespace smartStock.Api.Application.Common.Exceptions.Compras;

public sealed class CompraYaAnuladaException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Compra ya anulada";

    public CompraYaAnuladaException()
        : base("La compra ya fue anulada previamente y no puede volver a anularse.") { }
}
