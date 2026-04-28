using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using smartStock.Api.Application.Features.Usuarios.Admin.Commands.AltaProveedor;
using smartStock.Api.Application.Features.Usuarios.Admin.Commands.CambiarEstadoProveedor;
using smartStock.Api.Application.Features.Usuarios.Admin.Commands.EditarProveedor;
using smartStock.Api.Application.Features.Usuarios.Admin.Queries.ObtenerDetalleProveedor;
using smartStock.Api.Application.Features.Usuarios.Admin.Queries.ObtenerListaProveedores;

namespace smartStock.Api.Presentation.Controllers.Usuarios.Admin;

[ApiController]
[Route("api/administrador")]
[Authorize(Roles = "Administrador")]
public sealed class ProveedoresAdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProveedoresAdminController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// CU02-W1: Da de alta un nuevo proveedor en estado activo.
    /// </summary>
    [HttpPost("alta-proveedor")]
    [EnableRateLimiting("admin-escritura")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> AltaProveedor(
        [FromBody] AltaProveedorCommand command,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(
                User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var adminId))
            return Unauthorized();

        var commandConId = command with { UsuarioAltaId = adminId };
        var respuesta    = await _mediator.Send(commandConId, cancellationToken);
        return Created(string.Empty, respuesta);
    }

    /// <summary>
    /// CU02-W2: Edita los datos de un proveedor existente.
    /// </summary>
    [HttpPut("editar-proveedor/{id:guid}")]
    [EnableRateLimiting("admin-escritura")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> EditarProveedor(
        Guid id,
        [FromBody] EditarProveedorCommand command,
        CancellationToken cancellationToken)
    {
        var commandConId = command with { ProveedorId = id };
        var respuesta    = await _mediator.Send(commandConId, cancellationToken);
        return Ok(respuesta);
    }

    /// <summary>
    /// CU02-W3: Desactiva o reactiva un proveedor. Lanza 409 si el estado ya es el solicitado.
    /// </summary>
    [HttpPatch("cambiar-estado-proveedor/{id:guid}")]
    [EnableRateLimiting("admin-escritura")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> CambiarEstadoProveedor(
        Guid id,
        [FromBody] CambiarEstadoProveedorCommand command,
        CancellationToken cancellationToken)
    {
        var commandConId = command with { ProveedorId = id };
        var respuesta    = await _mediator.Send(commandConId, cancellationToken);
        return Ok(respuesta);
    }

    /// <summary>
    /// CU02-R1: Lista todos los proveedores con filtro opcional por estado y búsqueda por nombre, CUIT o email.
    /// </summary>
    [HttpGet("obtener-lista-proveedores")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObtenerListaProveedores(
        [FromQuery] string? filtroEstado,
        [FromQuery] string? busqueda,
        CancellationToken cancellationToken)
    {
        var respuesta = await _mediator.Send(
            new ObtenerListaProveedoresQuery(filtroEstado, busqueda), cancellationToken);
        return Ok(respuesta);
    }

    /// <summary>
    /// CU02-R2: Consulta el detalle completo de un proveedor por su Id.
    /// </summary>
    [HttpGet("obtener-detalle-proveedor/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerDetalleProveedor(
        Guid id,
        CancellationToken cancellationToken)
    {
        var respuesta = await _mediator.Send(new ObtenerDetalleProveedorQuery(id), cancellationToken);
        return Ok(respuesta);
    }
}
