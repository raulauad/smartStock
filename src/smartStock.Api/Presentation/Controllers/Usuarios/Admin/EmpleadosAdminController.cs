using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using smartStock.Api.Application.Features.Usuarios.Admin.Commands.AltaEmpleado;
using smartStock.Api.Application.Features.Usuarios.Admin.Commands.CambiarEstadoEmpleado;
using smartStock.Api.Application.Features.Usuarios.Admin.Commands.EliminarEmpleado;
using smartStock.Api.Application.Features.Usuarios.Admin.Queries.ObtenerDetalleEmpleado;
using smartStock.Api.Application.Features.Usuarios.Admin.Queries.ObtenerListaEmpleados;

namespace smartStock.Api.Presentation.Controllers.Usuarios.Admin;

[ApiController]
[Route("api/administrador")]
[Authorize(Roles = "Administrador")]
public sealed class EmpleadosAdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmpleadosAdminController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// CU01-W3: Alta de empleado (requiere sesión de Administrador).
    /// </summary>
    [HttpPost("alta-empleado")]
    [EnableRateLimiting("admin-escritura")]
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
    [HttpGet("obtener-lista-empleados")]
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
    [HttpGet("obtener-detalle-empleado/{id:guid}")]
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
    [HttpDelete("eliminar-empleado/{id:guid}")]
    [EnableRateLimiting("admin-escritura")]
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
    [HttpPatch("cambiar-estado-empleado/{id:guid}")]
    [EnableRateLimiting("admin-escritura")]
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
        var respuesta    = await _mediator.Send(commandConId, cancellationToken);
        return Ok(respuesta);
    }
}
