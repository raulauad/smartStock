using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smartStock.Application.Features.Commands.Usuarios.AltaEmpleado;
using smartStock.Application.Features.Commands.Usuarios.CambiarEstadoEmpleado;
using smartStock.Application.Features.Commands.Usuarios.EliminarEmpleado;
using smartStock.Application.Features.Commands.Usuarios.RegistrarAdmin;
using smartStock.Application.Features.Queries.Usuarios.ObtenerDetalleEmpleado;
using smartStock.Application.Features.Queries.Usuarios.ObtenerListaEmpleados;
using smartStock.Application.Features.Queries.Usuarios.ObtenerPerfilAdmin;

namespace smartStock.Presentation.Controllers.Usuarios.Admin;

[ApiController]
[Route("api/administrador")]
public class AdministradorController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdministradorController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// CU01-W1: Registra al administrador del sistema (operación única).
    /// </summary>
    [HttpPost("registro")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Registrar(
        [FromBody] RegistrarAdministradorCommand command,
        CancellationToken cancellationToken)
    {
        var respuesta = await _mediator.Send(command, cancellationToken);
        return Created(string.Empty, respuesta);
    }

    /// <summary>
    /// CU01-R1: Consulta del perfil del administrador autenticado.
    /// </summary>
    [HttpGet("perfil")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerPerfil(CancellationToken cancellationToken)
    {
        var adminId  = Guid.Parse(User.FindFirstValue("sub")
                       ?? User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var respuesta = await _mediator.Send(new ObtenerPerfilAdminQuery(adminId), cancellationToken);
        return Ok(respuesta);
    }

    /// <summary>
    /// CU01-W3: Alta de empleado (requiere sesión de Administrador).
    /// </summary>
    [HttpPost("empleado")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> AltaEmpleado(
        [FromBody] AltaEmpleadoCommand command,
        CancellationToken cancellationToken)
    {
        var respuesta = await _mediator.Send(command, cancellationToken);
        return Created(string.Empty, respuesta);
    }

    /// <summary>
    /// CU01-R2: Lista todos los empleados con su estado activo/inactivo.
    /// </summary>
    [HttpGet("empleados")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObtenerListaEmpleados(CancellationToken cancellationToken)
    {
        var respuesta = await _mediator.Send(new ObtenerListaEmpleadosQuery(), cancellationToken);
        return Ok(respuesta);
    }

    /// <summary>
    /// CU01-R2: Consulta el detalle completo de un empleado por su Id.
    /// </summary>
    [HttpGet("empleados/{id:guid}")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerDetalleEmpleado(
        Guid id,
        CancellationToken cancellationToken)
    {
        var respuesta = await _mediator.Send(new ObtenerDetalleEmpleadoQuery(id), cancellationToken);
        return Ok(respuesta);
    }

    /// <summary>
    /// CU01-W6: Elimina permanentemente la cuenta de un empleado.
    /// </summary>
    [HttpDelete("empleados/{id:guid}")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EliminarEmpleado(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new EliminarEmpleadoCommand { EmpleadoId = id }, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// CU01-W4 / CU01-W5: El administrador activa o suspende un empleado.
    /// Lanza 409 si el estado solicitado ya es el actual.
    /// </summary>
    [HttpPatch("empleados/{id:guid}/estado")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CambiarEstadoEmpleado(
        Guid id,
        [FromBody] CambiarEstadoEmpleadoCommand command,
        CancellationToken cancellationToken)
    {
        var commandConId = command with { EmpleadoId = id };
        var respuesta = await _mediator.Send(commandConId, cancellationToken);
        return Ok(respuesta);
    }
}
