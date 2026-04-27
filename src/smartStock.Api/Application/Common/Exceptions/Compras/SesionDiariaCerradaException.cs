using Microsoft.AspNetCore.Http;

namespace smartStock.Api.Application.Common.Exceptions.Compras;

public sealed class SesionDiariaCerradaException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Sesión diaria cerrada";

    public SesionDiariaCerradaException()
        : base("La sesión diaria de compras para esa fecha ya está cerrada. " +
               "Para corregir información de un día cerrado use el Ajuste manual de stock.") { }
}
