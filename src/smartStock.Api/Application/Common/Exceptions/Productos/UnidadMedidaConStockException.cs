using Microsoft.AspNetCore.Http;

namespace smartStock.Api.Application.Common.Exceptions.Productos;

public sealed class UnidadMedidaConStockException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status422UnprocessableEntity;
    public string Titulo     => "Cambio de unidad de medida con stock existente";

    public UnidadMedidaConStockException()
        : base("No se puede cambiar la unidad de medida de un producto que tiene stock distinto de cero. " +
               "Ajuste el stock a cero antes de modificar la unidad de medida.") { }
}
