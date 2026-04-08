using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using smartStock.Api.Application.Common.Exceptions;
using smartStock.Api.Application.Common.Interfaces;
using IAspNetExceptionHandler = Microsoft.AspNetCore.Diagnostics.IExceptionHandler;

namespace smartStock.Api.Application.Common.Middleware;

public sealed class GlobalExceptionHandler : IAspNetExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        => _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext       httpContext,
        Exception         exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = exception switch
        {
            ValidationException ve     => BuildValidacion(httpContext, ve),
            IExceptionHandler de => BuildDominio(httpContext, de, exception),
            _                          => BuildInterno(httpContext, exception)
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }

    private static ProblemDetails BuildValidacion(HttpContext ctx, ValidationException ex)
    {
        ctx.Response.StatusCode = StatusCodes.Status400BadRequest;

        var errores = ex.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray());

        var pd = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title  = "Error de validación",
            Detail = "Uno o más campos no superaron la validación."
        };
        pd.Extensions["errores"] = errores;
        return pd;
    }

    private static ProblemDetails BuildDominio(HttpContext ctx, IExceptionHandler de, Exception ex)
    {
        ctx.Response.StatusCode = de.CodigoHttp;

        var pd = new ProblemDetails
        {
            Status = de.CodigoHttp,
            Title  = de.Titulo,
            Detail = ex.Message
        };

        if (ex is IdentityException ie)
            pd.Extensions["errores"] = ie.Errores;

        return pd;
    }

    private ProblemDetails BuildInterno(HttpContext ctx, Exception ex)
    {
        ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;

        _logger.LogError(ex, "Excepción no controlada: {Tipo} — {Mensaje}", ex.GetType().Name, ex.Message);

        return new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title  = "Error interno del servidor",
            Detail = "Ocurrió un error inesperado. Intente nuevamente más tarde."
        };
    }
}
