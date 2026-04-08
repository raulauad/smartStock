using Microsoft.AspNetCore.Http;
using smartStock.Api.Application.Common.Interfaces;

namespace smartStock.Api.Application.Common.Exceptions.Usuarios;

public sealed class UsuarioNoEncontradoException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status404NotFound;
    public string Titulo     => "Usuario no encontrado";

    public UsuarioNoEncontradoException()
        : base("No se encontró el usuario solicitado.") { }
}
