using Microsoft.AspNetCore.Http;

namespace smartStock.Api.Application.Common.Exceptions.Compras;

public sealed class FechaCompraRetroactivaException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Fecha de compra retroactiva";

    public FechaCompraRetroactivaException()
        : base("La fecha de la compra es anterior al día actual. " +
               "Para confirmar la carga retroactiva, reenvíe la solicitud con 'confirmarFechaRetroactiva' en true.") { }
}
