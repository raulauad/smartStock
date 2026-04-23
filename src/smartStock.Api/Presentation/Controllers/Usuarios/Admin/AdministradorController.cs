using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
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
using smartStock.Api.Application.Features.Admin.Commands.AltaCategoria;
using smartStock.Api.Application.Features.Admin.Commands.EditarCategoria;
using smartStock.Api.Application.Features.Admin.Commands.CambiarEstadoCategoria;
using smartStock.Api.Application.Features.Admin.Queries.ObtenerListaCategorias;
using smartStock.Api.Application.Features.Admin.Queries.ObtenerDetalleCategoria;
using smartStock.Api.Application.Features.Admin.Commands.AltaProducto;
using smartStock.Api.Application.Features.Admin.Commands.EditarProducto;
using smartStock.Api.Application.Features.Admin.Commands.CambiarEstadoProducto;
using smartStock.Api.Application.Features.Admin.Commands.AgregarCodigoProducto;
using smartStock.Api.Application.Features.Admin.Commands.EditarCodigoProducto;
using smartStock.Api.Application.Features.Admin.Commands.EliminarCodigoProducto;
using smartStock.Api.Application.Features.Admin.Queries.ObtenerListaProductos;
using smartStock.Api.Application.Features.Admin.Queries.ObtenerDetalleProducto;

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
    [EnableRateLimiting("login")]
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
        if (!Guid.TryParse(
                User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var adminId))
            return Unauthorized();

        var respuesta = await _mediator.Send(new ObtenerPerfilAdminQuery(adminId), cancellationToken);
        return Ok(respuesta);
    }

    /// <summary>
    /// CU01-W3: Alta de empleado (requiere sesión de Administrador).
    /// </summary>
    [HttpPost("alta-empleado")]
    [Authorize(Roles = "Administrador")]
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
    [Authorize(Roles = "Administrador")]
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
        var respuesta = await _mediator.Send(commandConId, cancellationToken);
        return Ok(respuesta);
    }

    // ── Proveedores ───────────────────────────────────────────────────────────

    /// <summary>
    /// CU02-W1: Da de alta un nuevo proveedor en estado activo.
    /// </summary>
    [HttpPost("alta-proveedor")]
    [Authorize(Roles = "Administrador")]
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
    [Authorize(Roles = "Administrador")]
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
    [Authorize(Roles = "Administrador")]
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

    // ── Categorías ────────────────────────────────────────────────────────────

    /// <summary>
    /// CU03-W1: Da de alta una nueva categoría en estado activo.
    /// </summary>
    [HttpPost("alta-categoria")]
    [Authorize(Roles = "Administrador")]
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
    [Authorize(Roles = "Administrador")]
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
    [Authorize(Roles = "Administrador")]
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
    [Authorize(Roles = "Administrador")]
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
    [Authorize(Roles = "Administrador")]
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

    // ── Productos ─────────────────────────────────────────────────────────────

    /// <summary>
    /// CU04-W1: Da de alta un nuevo producto en estado activo, con al menos un código asignado.
    /// FA3: si precioVenta &lt; precioCosto y confirmarPrecioVentaMenorCosto = false → 422.
    /// FA4: si nombre coincide case-insensitive con otro producto activo y confirmarNombreSimilar = false → 409.
    /// FA2: si algún código ya existe en el sistema → 409.
    /// </summary>
    [HttpPost("alta-producto")]
    [Authorize(Roles = "Administrador")]
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
    [Authorize(Roles = "Administrador")]
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
    [Authorize(Roles = "Administrador")]
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
    [Authorize(Roles = "Administrador")]
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
    [Authorize(Roles = "Administrador")]
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
    [Authorize(Roles = "Administrador")]
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
    [Authorize(Roles = "Administrador")]
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
    [Authorize(Roles = "Administrador")]
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
