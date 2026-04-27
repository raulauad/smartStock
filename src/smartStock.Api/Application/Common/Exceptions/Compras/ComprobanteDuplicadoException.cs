using Microsoft.AspNetCore.Http;

namespace smartStock.Api.Application.Common.Exceptions.Compras;

public sealed class ComprobanteDuplicadoException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Comprobante duplicado";

    public ComprobanteDuplicadoException(string numero, string tipo)
        : base($"Ya existe una compra registrada para ese proveedor con el comprobante {tipo} N° {numero}. " +
               "Verifique los datos o cancele la operación.") { }
}
