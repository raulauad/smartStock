using Microsoft.AspNetCore.Http;

namespace smartStock.Api.Application.Common.Exceptions.Productos;

public sealed class NombreSimilarProductoException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Nombre similar a producto existente";

    public NombreSimilarProductoException(IEnumerable<string> nombresSimilares)
        : base($"El nombre ingresado coincide con productos existentes: {string.Join(", ", nombresSimilares)}. " +
               "Para confirmar que se trata de un producto distinto, reenvíe la solicitud con confirmarNombreSimilar = true.") { }
}
