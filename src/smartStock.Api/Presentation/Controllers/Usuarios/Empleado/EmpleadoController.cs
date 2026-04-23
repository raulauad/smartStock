using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using smartStock.Api.Application.Features.Empleados.Commands.CambiarContrasena;
using smartStock.Api.Application.Features.Empleados.Commands.EditarPerfilEmpleado;

namespace smartStock.Api.Presentation.Controllers.Usuarios.Empleado;

[ApiController]
[Route("api/empleado")]
[Authorize(Roles = "Empleado")]
public class EmpleadoController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmpleadoController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// CU01-W4: El empleado edita sus propios datos de perfil.
    /// El Id se extrae del JWT; no puede ser enviado ni modificado desde el body.
    /// </summary>
    [HttpPut("editar-perfil-empleado")]
    [EnableRateLimiting("admin-escritura")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> EditarPerfilEmpleado(
        [FromBody] EditarPerfilEmpleadoCommand command,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(
                User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var usuarioId))
            return Unauthorized();

        var commandConId = command with { UsuarioId = usuarioId };

        var respuesta = await _mediator.Send(commandConId, cancellationToken);
        return Ok(respuesta);
    }

    /// <summary>
    /// CU01-W7: El empleado autenticado cambia su propia contraseña.
    /// El Id se extrae del JWT; no puede ser enviado ni modificado desde el body.
    /// </summary>
    [HttpPatch("cambiar-contrasena")]
    [EnableRateLimiting("admin-escritura")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CambiarContrasena(
        [FromBody] CambiarContrasenaCommand command,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(
                User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var usuarioId))
            return Unauthorized();

        var commandConId = command with { UsuarioId = usuarioId };

        var respuesta = await _mediator.Send(commandConId, cancellationToken);
        return Ok(respuesta);
    }
}
