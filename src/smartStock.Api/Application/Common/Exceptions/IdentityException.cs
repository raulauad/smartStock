using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using smartStock.Api.Application.Common.Interfaces;

namespace smartStock.Api.Application.Common.Exceptions;

public sealed class IdentityException : Exception, IExceptionHandler
{
    public int    CodigoHttp => StatusCodes.Status422UnprocessableEntity;
    public string Titulo     => "Error al crear el usuario";

    public IReadOnlyList<string> Errores { get; }

    public IdentityException(IEnumerable<IdentityError> errores)
        : base("Error al crear el usuario en el sistema de identidad.")
    {
        Errores = errores.Select(e => e.Description).ToList();
    }
}
