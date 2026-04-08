using Microsoft.AspNetCore.Http;
using smartStock.Api.Application.Common.Interfaces;

namespace smartStock.Api.Application.Common.Exceptions.Usuarios;

public sealed class DniDuplicadoException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status409Conflict;
    public string Titulo     => "DNI ya registrado";

    public DniDuplicadoException()
        : base("Ya existe un usuario registrado con ese número de DNI.") { }
}
