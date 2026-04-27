using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using smartStock.Api.Application.Features.Compras.Commands.AjusteManualStock;
using smartStock.Api.Application.Features.Compras.Commands.AnularCompra;
using smartStock.Api.Application.Features.Compras.Commands.RegistrarCompra;
using smartStock.Api.Application.Features.Compras.Queries.ObtenerAjustesStock;
using smartStock.Api.Application.Features.Compras.Queries.ObtenerDetalleCompra;
using smartStock.Api.Application.Features.Compras.Queries.ObtenerListaCompras;
using smartStock.Api.Application.Features.Compras.Queries.ObtenerSesionDiariaCompras;

namespace smartStock.Api.Presentation.Controllers.Compras;

[ApiController]
[Route("api/compras")]
[Authorize]
public sealed class ComprasController : ControllerBase
{
    private readonly IMediator _mediator;

    public ComprasController(IMediator mediator) => _mediator = mediator;

    /// <summary>CU05-W1: Registrar una nueva compra. Crea la sesión diaria si no existe.</summary>
    [HttpPost("registrar-compra")]
    [EnableRateLimiting("admin-escritura")]
    public async Task<IActionResult> RegistrarCompra(
        [FromBody] RegistrarCompraCommand command,
        CancellationToken ct)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId))
            return Unauthorized();

        var result = await _mediator.Send(command with { UsuarioId = usuarioId }, ct);
        return Ok(result);
    }

    /// <summary>CU05-W2: Anular una compra vigente dentro de una sesión abierta.</summary>
    [HttpPatch("anular-compra/{id:int}")]
    [Authorize(Roles = "Administrador")]
    [EnableRateLimiting("admin-escritura")]
    public async Task<IActionResult> AnularCompra(
        [FromRoute] int id,
        [FromBody] AnularCompraCommand command,
        CancellationToken ct)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId))
            return Unauthorized();

        var result = await _mediator.Send(command with { CompraId = id, UsuarioId = usuarioId }, ct);
        return Ok(result);
    }

    /// <summary>CU05-W3: Ajuste manual de stock (incremento o decremento con motivo obligatorio).</summary>
    [HttpPost("ajuste-manual-stock")]
    [Authorize(Roles = "Administrador")]
    [EnableRateLimiting("admin-escritura")]
    public async Task<IActionResult> AjusteManualStock(
        [FromBody] AjusteManualStockCommand command,
        CancellationToken ct)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId))
            return Unauthorized();

        var result = await _mediator.Send(command with { UsuarioId = usuarioId }, ct);
        return Ok(result);
    }

    /// <summary>CU05-R1: Listado de compras con filtros opcionales.</summary>
    [HttpGet("obtener-lista-compras")]
    public async Task<IActionResult> ObtenerListaCompras(
        [FromQuery] DateTime? fechaDesde,
        [FromQuery] DateTime? fechaHasta,
        [FromQuery] Guid?     proveedorId,
        [FromQuery] Guid?     usuarioRegistroId,
        [FromQuery] string?   filtroEstado,
        [FromQuery] string?   numeroComprobante,
        CancellationToken ct)
    {
        var result = await _mediator.Send(
            new ObtenerListaComprasQuery(fechaDesde, fechaHasta, proveedorId, usuarioRegistroId, filtroEstado, numeroComprobante), ct);
        return Ok(result);
    }

    /// <summary>CU05-R2: Detalle completo de una compra.</summary>
    [HttpGet("obtener-detalle-compra/{id:int}")]
    public async Task<IActionResult> ObtenerDetalleCompra(
        [FromRoute] int id,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new ObtenerDetalleCompraQuery(id), ct);
        return Ok(result);
    }

    /// <summary>CU05-R3: Sesión diaria de compras. Sin parámetro devuelve la del día actual.</summary>
    [HttpGet("obtener-sesion-diaria-compras")]
    public async Task<IActionResult> ObtenerSesionDiariaCompras(
        [FromQuery] DateTime? fecha,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new ObtenerSesionDiariaComprasQuery(fecha), ct);
        if (result is null) return NotFound();
        return Ok(result);
    }

    /// <summary>CU05-R4: Historial de ajustes y movimientos compensatorios de stock.</summary>
    [HttpGet("obtener-ajustes-stock")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> ObtenerAjustesStock(
        [FromQuery] DateTime? fechaDesde,
        [FromQuery] DateTime? fechaHasta,
        [FromQuery] Guid?     productoId,
        [FromQuery] Guid?     usuarioId,
        [FromQuery] string?   filtroTipo,
        CancellationToken ct)
    {
        var result = await _mediator.Send(
            new ObtenerAjustesStockQuery(fechaDesde, fechaHasta, productoId, usuarioId, filtroTipo), ct);
        return Ok(result);
    }
}
