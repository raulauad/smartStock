using Microsoft.AspNetCore.Http;
using smartStock.Application.Common.Interfaces;

namespace smartStock.Application.Common.Exceptions.Usuarios;

public sealed class EmailDuplicadoException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "Email ya registrado";

    public EmailDuplicadoException()
        : base("Ya existe un usuario registrado con ese email.") { }
}
