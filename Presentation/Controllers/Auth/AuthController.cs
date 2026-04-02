using MediatR;
using Microsoft.AspNetCore.Mvc;
using smartStock.Application.Features.Commands.Usuarios.IniciarSesion;

namespace smartStock.Presentation.Controllers.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// CU01-W2: Inicio de sesión para Administrador y Empleado.
    /// </summary>
    [HttpPost("login")]
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
}
