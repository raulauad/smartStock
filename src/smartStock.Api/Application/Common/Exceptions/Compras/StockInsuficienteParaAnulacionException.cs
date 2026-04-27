using Microsoft.AspNetCore.Http;

namespace smartStock.Api.Application.Common.Exceptions.Compras;

public sealed class StockInsuficienteParaAnulacionException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Stock insuficiente para revertir la compra";

    public StockInsuficienteParaAnulacionException(string nombreProducto, decimal stockActual, decimal cantidadRequerida)
        : base($"El producto '{nombreProducto}' no tiene stock suficiente para revertir la compra. " +
               $"Stock actual: {stockActual}, cantidad a descontar: {cantidadRequerida}.") { }
}
