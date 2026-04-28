using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using smartStock.Api.Application.Features.Usuarios.Admin.Commands.AgregarCodigoProducto;
using smartStock.Api.Application.Features.Usuarios.Admin.Commands.AltaProducto;
using smartStock.Api.Application.Features.Usuarios.Admin.Commands.CambiarEstadoProducto;
using smartStock.Api.Application.Features.Usuarios.Admin.Commands.EditarCodigoProducto;
using smartStock.Api.Application.Features.Usuarios.Admin.Commands.EditarProducto;
using smartStock.Api.Application.Features.Usuarios.Admin.Commands.EliminarCodigoProducto;
using smartStock.Api.Application.Features.Usuarios.Admin.Queries.ObtenerDetalleProducto;
using smartStock.Api.Application.Features.Usuarios.Admin.Queries.ObtenerListaProductos;

namespace smartStock.Api.Presentation.Controllers.Usuarios.Admin;

[ApiController]
[Route("api/administrador")]
[Authorize(Roles = "Administrador")]
public sealed class ProductosAdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductosAdminController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// CU04-W1: Da de alta un nuevo producto en estado activo, con al menos un código asignado.
    /// FA3: si precioVenta &lt; precioCosto y confirmarPrecioVentaMenorCosto = false → 422.
    /// FA4: si nombre coincide case-insensitive con otro producto activo y confirmarNombreSimilar = false → 409.
    /// FA2: si algún código ya existe en el sistema → 409.
    /// </summary>
    [HttpPost("alta-producto")]
    [EnableRateLimiting("admin-escritura")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> AltaProducto(
        [FromBody] AltaProductoCommand command,
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
    /// CU04-W2: Edita datos de un producto existente (Nombre, Categoría, UnidadMedida, precios, StockMínimo).
    /// FA3: bloquea cambio de unidad de medida si stock != 0 → 422.
    /// </summary>
    [HttpPut("editar-producto/{id:guid}")]
    [EnableRateLimiting("admin-escritura")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> EditarProducto(
        Guid id,
        [FromBody] EditarProductoCommand command,
        CancellationToken cancellationToken)
    {
        var commandConId = command with { ProductoId = id };
        var respuesta    = await _mediator.Send(commandConId, cancellationToken);
        return Ok(respuesta);
    }

    /// <summary>
    /// CU04-W3: Desactiva o reactiva un producto. Lanza 409 si el estado ya es el solicitado.
    /// La respuesta incluye StockActual para que el cliente muestre la advertencia de stock si corresponde.
    /// </summary>
    [HttpPatch("cambiar-estado-producto/{id:guid}")]
    [EnableRateLimiting("admin-escritura")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> CambiarEstadoProducto(
        Guid id,
        [FromBody] CambiarEstadoProductoCommand command,
        CancellationToken cancellationToken)
    {
        var commandConId = command with { ProductoId = id };
        var respuesta    = await _mediator.Send(commandConId, cancellationToken);
        return Ok(respuesta);
    }

    /// <summary>
    /// CU04-W4 (agregar): Agrega un nuevo código a un producto existente. Lanza 409 si el código ya existe.
    /// </summary>
    [HttpPost("agregar-codigo-producto/{id:guid}")]
    [EnableRateLimiting("admin-escritura")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> AgregarCodigoProducto(
        Guid id,
        [FromBody] AgregarCodigoProductoCommand command,
        CancellationToken cancellationToken)
    {
        var commandConId = command with { ProductoId = id };
        var respuesta    = await _mediator.Send(commandConId, cancellationToken);
        return Created(string.Empty, respuesta);
    }

    /// <summary>
    /// CU04-W4 (editar): Edita Factor y Descripción de un código existente. El código y Tipo no son editables.
    /// </summary>
    [HttpPatch("editar-codigo-producto/{id:guid}/{codigoId:guid}")]
    [EnableRateLimiting("admin-escritura")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> EditarCodigoProducto(
        Guid id,
        Guid codigoId,
        [FromBody] EditarCodigoProductoCommand command,
        CancellationToken cancellationToken)
    {
        var commandConIds = command with { ProductoId = id, CodigoId = codigoId };
        var respuesta     = await _mediator.Send(commandConIds, cancellationToken);
        return Ok(respuesta);
    }

    /// <summary>
    /// CU04-W4 (eliminar): Elimina un código de un producto. Lanza 409 si es el único código del producto.
    /// </summary>
    [HttpDelete("eliminar-codigo-producto/{id:guid}/{codigoId:guid}")]
    [EnableRateLimiting("admin-escritura")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> EliminarCodigoProducto(
        Guid id,
        Guid codigoId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new EliminarCodigoProductoCommand { ProductoId = id, CodigoId = codigoId },
            cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// CU04-R1: Lista productos con filtros opcionales por estado, categoría, alerta de stock bajo y búsqueda por nombre o código.
    /// </summary>
    [HttpGet("obtener-lista-productos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObtenerListaProductos(
        [FromQuery] string? filtroEstado,
        [FromQuery] Guid?   filtroCategoria,
        [FromQuery] bool?   alertaStockBajo,
        [FromQuery] string? busqueda,
        CancellationToken cancellationToken)
    {
        var respuesta = await _mediator.Send(
            new ObtenerListaProductosQuery(filtroEstado, filtroCategoria, alertaStockBajo, busqueda),
            cancellationToken);
        return Ok(respuesta);
    }

    /// <summary>
    /// CU04-R2: Consulta el detalle completo de un producto por su Id, incluyendo sus códigos.
    /// </summary>
    [HttpGet("obtener-detalle-producto/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerDetalleProducto(
        Guid id,
        CancellationToken cancellationToken)
    {
        var respuesta = await _mediator.Send(new ObtenerDetalleProductoQuery(id), cancellationToken);
        return Ok(respuesta);
    }
}
