using Microsoft.AspNetCore.Http;

namespace smartStock.Api.Application.Common.Exceptions.Ventas;

public sealed class StockInsuficienteParaVentaException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Stock insuficiente para completar la venta";

    public IReadOnlyList<string> ProductosConflicto { get; }

    public StockInsuficienteParaVentaException(IReadOnlyList<string> productosConflicto)
        : base("No hay stock suficiente para los siguientes productos: " +
               string.Join(", ", productosConflicto) + ". " +
               "El stock puede haber sido modificado por otra operación concurrente. Ajuste las cantidades e intente nuevamente.")
    {
        ProductosConflicto = productosConflicto;
    }
}
