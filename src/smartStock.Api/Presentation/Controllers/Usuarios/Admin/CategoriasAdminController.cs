using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using smartStock.Api.Application.Features.Usuarios.Admin.Commands.AltaCategoria;
using smartStock.Api.Application.Features.Usuarios.Admin.Commands.CambiarEstadoCategoria;
using smartStock.Api.Application.Features.Usuarios.Admin.Commands.EditarCategoria;
using smartStock.Api.Application.Features.Usuarios.Admin.Queries.ObtenerDetalleCategoria;
using smartStock.Api.Application.Features.Usuarios.Admin.Queries.ObtenerListaCategorias;

namespace smartStock.Api.Presentation.Controllers.Usuarios.Admin;

[ApiController]
[Route("api/administrador")]
[Authorize(Roles = "Administrador")]
public sealed class CategoriasAdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriasAdminController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// CU03-W1: Da de alta una nueva categoría en estado activo.
    /// </summary>
    [HttpPost("alta-categoria")]
    [EnableRateLimiting("admin-escritura")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> AltaCategoria(
        [FromBody] AltaCategoriaCommand command,
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
    /// CU03-W2: Edita los datos de una categoría existente.
    /// </summary>
    [HttpPut("editar-categoria/{id:guid}")]
    [EnableRateLimiting("admin-escritura")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> EditarCategoria(
        Guid id,
        [FromBody] EditarCategoriaCommand command,
        CancellationToken cancellationToken)
    {
        var commandConId = command with { CategoriaId = id };
        var respuesta    = await _mediator.Send(commandConId, cancellationToken);
        return Ok(respuesta);
    }

    /// <summary>
    /// CU03-W3: Desactiva o reactiva una categoría. Si tiene productos, requiere CategoriaDestinoId para reasignarlos.
    /// Lanza 409 si el estado ya es el solicitado o no hay categorías activas alternativas.
    /// Lanza 422 si tiene productos pero no se indicó destino, o el destino es inválido.
    /// </summary>
    [HttpPatch("cambiar-estado-categoria/{id:guid}")]
    [EnableRateLimiting("admin-escritura")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> CambiarEstadoCategoria(
        Guid id,
        [FromBody] CambiarEstadoCategoriaCommand command,
        CancellationToken cancellationToken)
    {
        var commandConId = command with { CategoriaId = id };
        var respuesta    = await _mediator.Send(commandConId, cancellationToken);
        return Ok(respuesta);
    }

    /// <summary>
    /// CU03-R1: Lista todas las categorías con filtro opcional por estado y búsqueda por nombre.
    /// </summary>
    [HttpGet("obtener-lista-categorias")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObtenerListaCategorias(
        [FromQuery] string? filtroEstado,
        [FromQuery] string? busqueda,
        CancellationToken cancellationToken)
    {
        var respuesta = await _mediator.Send(
            new ObtenerListaCategoriasQuery(filtroEstado, busqueda), cancellationToken);
        return Ok(respuesta);
    }

    /// <summary>
    /// CU03-R2: Consulta el detalle completo de una categoría por su Id.
    /// </summary>
    [HttpGet("obtener-detalle-categoria/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerDetalleCategoria(
        Guid id,
        CancellationToken cancellationToken)
    {
        var respuesta = await _mediator.Send(new ObtenerDetalleCategoriaQuery(id), cancellationToken);
        return Ok(respuesta);
    }
}
