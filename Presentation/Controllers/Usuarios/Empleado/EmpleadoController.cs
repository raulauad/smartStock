using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smartStock.Application.Features.Commands.Usuarios.EditarPerfilEmpleado;

namespace smartStock.Presentation.Controllers.Usuarios.Empleado;

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
    [HttpPut("perfil")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> EditarPerfil(
        [FromBody] EditarPerfilEmpleadoCommand command,
        CancellationToken cancellationToken)
    {
        var usuarioId = Guid.Parse(User.FindFirstValue("sub")
                        ?? User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // Se sobreescribe el UsuarioId con el del JWT para evitar manipulación
        var commandConId = command with { UsuarioId = usuarioId };

        var respuesta = await _mediator.Send(commandConId, cancellationToken);
        return Ok(respuesta);
    }
}
