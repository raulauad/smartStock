using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using smartStock.Api.Application.Features.Usuarios.Admin.Commands.RegistrarAdmin;
using smartStock.Api.Application.Features.Usuarios.Admin.Queries.ObtenerPerfilAdmin;

namespace smartStock.Api.Presentation.Controllers.Usuarios.Admin;

[ApiController]
[Route("api/administrador")]
public sealed class AdministradorController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdministradorController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// CU01-W1: Registra al administrador del sistema (operación única).
    /// </summary>
    [HttpPost("registrar-administrador")]
    [EnableRateLimiting("login")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> RegistrarAdministrador(
        [FromBody] RegistrarAdministradorCommand command,
        CancellationToken cancellationToken)
    {
        var respuesta = await _mediator.Send(command, cancellationToken);
        return Created(string.Empty, respuesta);
    }

    /// <summary>
    /// CU01-R1: Consulta del perfil del administrador autenticado.
    /// </summary>
    [HttpGet("obtener-perfil-admin")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerPerfilAdmin(CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(
                User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var adminId))
            return Unauthorized();

        var respuesta = await _mediator.Send(new ObtenerPerfilAdminQuery(adminId), cancellationToken);
        return Ok(respuesta);
    }
}
