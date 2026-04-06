using Microsoft.AspNetCore.Http;
using smartStock.Application.Common.Interfaces;

namespace smartStock.Application.Common.Exceptions.Usuarios;

public sealed class EstadoUsuarioSinCambioException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Estado sin cambio";

    public EstadoUsuarioSinCambioException(bool estaActivo)
        : base(estaActivo
            ? "El usuario ya se encuentra activo."
            : "El usuario ya se encuentra suspendido.") { }
}
