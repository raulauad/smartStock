using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smartStock.Api.Application.Features.Admin.Commands.AltaEmpleado;
using smartStock.Api.Application.Features.Admin.Commands.AltaProveedor;
using smartStock.Api.Application.Features.Admin.Commands.CambiarEstadoEmpleado;
using smartStock.Api.Application.Features.Admin.Commands.CambiarEstadoProveedor;
using smartStock.Api.Application.Features.Admin.Commands.EditarProveedor;
using smartStock.Api.Application.Features.Admin.Commands.EliminarEmpleado;
using smartStock.Api.Application.Features.Admin.Commands.RegistrarAdmin;
using smartStock.Api.Application.Features.Admin.Queries.ObtenerDetalleEmpleado;
using smartStock.Api.Application.Features.Admin.Queries.ObtenerDetalleProveedor;
using smartStock.Api.Application.Features.Admin.Queries.ObtenerListaEmpleados;
using smartStock.Api.Application.Features.Admin.Queries.ObtenerListaProveedores;
using smartStock.Api.Application.Features.Admin.Queries.ObtenerPerfilAdmin;

namespace smartStock.Api.Presentation.Controllers.Usuarios.Admin;

[ApiController]
[Route("api/administrador")]
public class AdministradorController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdministradorController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// CU01-W1: Registra al administrador del sistema (operación única).
    /// </summary>
    [HttpPost("registrar-administrador")]
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
        var adminId  = Guid.Parse(User.FindFirstValue("sub")
                       ?? User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var respuesta = await _mediator.Send(new ObtenerPerfilAdminQuery(adminId), cancellationToken);
        return Ok(respuesta);
    }

    /// <summary>
    /// CU01-W3: Alta de empleado (requiere sesión de Administrador).
    /// </summary>
    [HttpPost("alta-empleado")]
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
    [HttpGet("obtener-lista-empleados")]
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
    [HttpGet("obtener-detalle-empleado/{id:guid}")]
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
    [HttpDelete("eliminar-empleado/{id:guid}")]
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
    [HttpPatch("cambiar-estado-empleado/{id:guid}")]
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

    // ── Proveedores ───────────────────────────────────────────────────────────

    /// <summary>
    /// CU02-W1: Da de alta un nuevo proveedor en estado activo.
    /// </summary>
    [HttpPost("alta-proveedor")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AltaProveedor(
        [FromBody] AltaProveedorCommand command,
        CancellationToken cancellationToken)
    {
        var adminId       = Guid.Parse(User.FindFirstValue("sub")
                            ?? User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var commandConId  = command with { UsuarioAltaId = adminId };
        var respuesta     = await _mediator.Send(commandConId, cancellationToken);
        return Created(string.Empty, respuesta);
    }

    /// <summary>
    /// CU02-W2: Edita los datos de un proveedor existente.
    /// </summary>
    [HttpPut("editar-proveedor/{id:guid}")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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
    [Authorize(Roles = "Administrador")]
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
    [Authorize(Roles = "Administrador")]
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
