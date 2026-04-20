namespace smartStock.Api.Application.Common.Exceptions;

/// <summary>
/// Contrato que implementan las excepciones de dominio
/// para exponer su código HTTP y título semántico.
/// </summary>
public interface IExceptionHandler
{
    int    CodigoHttp { get; }
    string Titulo     { get; }
}
