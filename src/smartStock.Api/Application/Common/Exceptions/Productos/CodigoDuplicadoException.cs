using Microsoft.AspNetCore.Http;

namespace smartStock.Api.Application.Common.Exceptions.Productos;

public sealed class CodigoDuplicadoException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Código de producto duplicado";

    public CodigoDuplicadoException(string codigo)
        : base($"El código '{codigo}' ya está registrado en el sistema y pertenece a otro producto.") { }
}
