using Microsoft.AspNetCore.Http;

namespace smartStock.Api.Application.Common.Exceptions.Productos;

public sealed class EstadoProductoSinCambioException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Estado del producto sin cambio";

    public EstadoProductoSinCambioException()
        : base("El producto ya se encuentra en el estado solicitado.") { }
}
