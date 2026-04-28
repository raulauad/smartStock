using Microsoft.AspNetCore.Http;

namespace smartStock.Api.Application.Common.Exceptions.Ventas;

public sealed class SesionDiariaVentaCerradaException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Sesión diaria cerrada";

    public SesionDiariaVentaCerradaException()
        : base("La sesión diaria de ventas para esa fecha ya está cerrada. " +
               "Para corregir información de un día cerrado use el Ajuste manual de stock.") { }
}
