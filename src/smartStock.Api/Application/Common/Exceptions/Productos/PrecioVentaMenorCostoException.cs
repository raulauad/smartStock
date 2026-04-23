using Microsoft.AspNetCore.Http;

namespace smartStock.Api.Application.Common.Exceptions.Productos;

public sealed class PrecioVentaMenorCostoException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status422UnprocessableEntity;
    public string Titulo     => "Precio de venta menor al precio de costo";

    public PrecioVentaMenorCostoException()
        : base("El precio de venta es menor al precio de costo. " +
               "Para confirmar esta configuración, reenvíe la solicitud con confirmarPrecioVentaMenorCosto = true.") { }
}
