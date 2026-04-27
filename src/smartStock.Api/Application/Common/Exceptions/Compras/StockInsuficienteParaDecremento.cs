using Microsoft.AspNetCore.Http;

namespace smartStock.Api.Application.Common.Exceptions.Compras;

public sealed class StockInsuficienteParaDecremento : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Stock insuficiente para el decremento";

    public StockInsuficienteParaDecremento(decimal stockActual)
        : base($"La cantidad a decrementar supera el stock actual ({stockActual}). " +
               "No se permite dejar el stock en negativo mediante un ajuste.") { }
}
