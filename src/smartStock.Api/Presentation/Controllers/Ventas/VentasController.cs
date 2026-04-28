using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using smartStock.Api.Application.Features.Ventas.Commands.AnularVenta;
using smartStock.Api.Application.Features.Ventas.Commands.RegistrarVenta;
using smartStock.Api.Application.Features.Ventas.Queries.ObtenerDetalleVenta;
using smartStock.Api.Application.Features.Ventas.Queries.ObtenerListaVentas;
using smartStock.Api.Application.Features.Ventas.Queries.ObtenerSesionDiariaVentas;
using smartStock.Api.Application.Features.Ventas.Queries.ProductoPorCodigo;

namespace smartStock.Api.Presentation.Controllers.Ventas;

[ApiController]
[Route("api/ventas")]
[Authorize]
public sealed class VentasController : ControllerBase
{
    private readonly IMediator _mediator;

    public VentasController(IMediator mediator) => _mediator = mediator;

    /// <summary>CU06-W1: Registrar una nueva venta. Crea la sesión diaria si no existe.</summary>
    [HttpPost("registrar-venta")]
    [EnableRateLimiting("admin-escritura")]
    public async Task<IActionResult> RegistrarVenta(
        [FromBody] RegistrarVentaCommand command,
        CancellationToken ct)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId))
            return Unauthorized();

        var result = await _mediator.Send(command with { UsuarioId = usuarioId }, ct);
        return Ok(result);
    }

    /// <summary>CU06-W2: Anular una venta vigente dentro de una sesión abierta.</summary>
    [HttpPatch("anular-venta/{id:int}")]
    [Authorize(Roles = "Administrador")]
    [EnableRateLimiting("admin-escritura")]
    public async Task<IActionResult> AnularVenta(
        [FromRoute] int id,
        [FromBody] AnularVentaCommand command,
        CancellationToken ct)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId))
            return Unauthorized();

        var result = await _mediator.Send(command with { VentaId = id, UsuarioId = usuarioId }, ct);
        return Ok(result);
    }

    /// <summary>CU06-R1: Listado de ventas con filtros opcionales.</summary>
    [HttpGet("obtener-lista-ventas")]
    public async Task<IActionResult> ObtenerListaVentas(
        [FromQuery] DateTime? fechaDesde,
        [FromQuery] DateTime? fechaHasta,
        [FromQuery] string?   filtroFormaPago,
        [FromQuery] Guid?     usuarioRegistroId,
        [FromQuery] string?   filtroEstado,
        [FromQuery] int?      numeroComprobante,
        CancellationToken ct)
    {
        var esAdmin = User.IsInRole("Administrador");
        var result  = await _mediator.Send(
            new ObtenerListaVentasQuery(fechaDesde, fechaHasta, filtroFormaPago, usuarioRegistroId, filtroEstado, numeroComprobante, esAdmin), ct);
        return Ok(result);
    }

    /// <summary>CU06-R2: Detalle completo de una venta.</summary>
    [HttpGet("obtener-detalle-venta/{id:int}")]
    public async Task<IActionResult> ObtenerDetalleVenta(
        [FromRoute] int id,
        CancellationToken ct)
    {
        var esAdmin = User.IsInRole("Administrador");
        var result  = await _mediator.Send(new ObtenerDetalleVentaQuery(id, esAdmin), ct);
        if (result is null) return NotFound();
        return Ok(result);
    }

    /// <summary>CU06-R3: Sesión diaria de ventas. Sin parámetro devuelve la del día actual.</summary>
    [HttpGet("obtener-sesion-diaria-ventas")]
    public async Task<IActionResult> ObtenerSesionDiariaVentas(
        [FromQuery] DateTime? fecha,
        CancellationToken ct)
    {
        var esAdmin = User.IsInRole("Administrador");
        var result  = await _mediator.Send(new ObtenerSesionDiariaVentasQuery(fecha, esAdmin), ct);
        if (result is null) return NotFound();
        return Ok(result);
    }

    /// <summary>CU06-R4: Lookup rápido de producto por código exacto (para flujo POS).</summary>
    [HttpGet("producto-por-codigo")]
    public async Task<IActionResult> ProductoPorCodigo(
        [FromQuery] string codigo,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            return BadRequest("El parámetro 'codigo' es requerido.");

        var esAdmin = User.IsInRole("Administrador");
        var result  = await _mediator.Send(new ProductoPorCodigoQuery(codigo, esAdmin), ct);
        if (result is null) return NotFound();
        return Ok(result);
    }
}
