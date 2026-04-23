using System.IdentityModel.Tokens.Jwt;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using smartStock.Api.Application.Features.Auth.Commands.CerrarSesion;
using smartStock.Api.Application.Features.Auth.Commands.IniciarSesion;

namespace smartStock.Api.Presentation.Controllers.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// CU01-W2: Inicio de sesión para Administrador y Empleado.
    /// </summary>
    [EnableRateLimiting("login")]
    [HttpPost("iniciar-sesion")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> IniciarSesion(
        [FromBody] IniciarSesionCommand command,
        CancellationToken cancellationToken)
    {
        var respuesta = await _mediator.Send(command, cancellationToken);
        return Ok(respuesta);
    }

    /// <summary>
    /// CU01-W8: Cierre de sesión para Administrador y Empleado. Revoca el JWT activo.
    /// </summary>
    [Authorize]
    [HttpPost("cerrar-sesion")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CerrarSesion(CancellationToken cancellationToken)
    {
        var jti      = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? string.Empty;
        var subClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? Guid.Empty.ToString();
        var expClaim = User.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;

        var expiracion = expClaim is not null && long.TryParse(expClaim, out var expUnix)
            ? DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime
            : DateTime.UtcNow;

        if (!Guid.TryParse(subClaim, out var usuarioId))
            return Unauthorized();

        var command = new CerrarSesionCommand
        {
            Jti        = jti,
            Expiracion = expiracion,
            UsuarioId  = usuarioId
        };

        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
