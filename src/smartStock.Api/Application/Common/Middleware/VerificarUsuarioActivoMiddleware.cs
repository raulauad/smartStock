using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smartStock.Api.Application.Common.Interfaces.Repositories;

namespace smartStock.Api.Application.Common.Middleware;

public sealed class VerificarUsuarioActivoMiddleware
{
    private readonly RequestDelegate _next;

    public VerificarUsuarioActivoMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, IUsuarioRepository usuarioRepository)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var idClaim = context.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (Guid.TryParse(idClaim, out var userId) &&
                !await usuarioRepository.EstaActivoAsync(userId, context.RequestAborted))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = StatusCodes.Status403Forbidden,
                    Title  = "Cuenta inactiva",
                    Detail = "La cuenta no está activa. Contacte al administrador."
                });
                return;
            }
        }

        await _next(context);
    }
}
