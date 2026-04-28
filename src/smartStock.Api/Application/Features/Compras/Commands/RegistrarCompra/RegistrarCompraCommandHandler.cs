using MediatR;
using smartStock.Api.Application.Common.Exceptions.Compras;
using smartStock.Api.Application.Common.Exceptions.Proveedores;
using smartStock.Api.Application.Common.Exceptions.Productos;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Enums;
using smartStock.Api.Domain.Models;
using smartStock.Shared.Dtos.Compras.RegistrarCompra;

namespace smartStock.Api.Application.Features.Compras.Commands.RegistrarCompra;

public sealed class RegistrarCompraCommandHandler
    : IRequestHandler<RegistrarCompraCommand, RegistrarCompraResponse>
{
    private readonly ICompraRepository   _compraRepo;
    private readonly IProductoRepository _productoRepo;
    private readonly IProveedorRepository _proveedorRepo;

    public RegistrarCompraCommandHandler(
        ICompraRepository    compraRepo,
        IProductoRepository  productoRepo,
        IProveedorRepository proveedorRepo)
    {
        _compraRepo    = compraRepo;
        _productoRepo  = productoRepo;
        _proveedorRepo = proveedorRepo;
    }

    public async Task<RegistrarCompraResponse> Handle(RegistrarCompraCommand command, CancellationToken ct)
    {
        // FA5: advertencia fecha retroactiva
        var hoy = DateTime.UtcNow.Date;
        if (command.FechaCompra.Date < hoy && !command.ConfirmarFechaRetroactiva)
            throw new FechaCompraRetroactivaException();

        // Validar proveedor activo
        var proveedor = await _proveedorRepo.ObtenerPorIdAsync(command.ProveedorId, ct);
        if (proveedor is null || !proveedor.EstaActivo)
            throw new ProveedorNoEncontradoException();

        // Obtener o crear sesión diaria para la fecha de la compra
        var fechaSesion = command.FechaCompra.Date;
        var sesion      = await _compraRepo.ObtenerSesionDiariaPorFechaAsync(fechaSesion, ct);
        var esNuevaSesion = sesion is null;

        if (sesion is null)
        {
            sesion = new CompraDia
            {
                FechaSesion = fechaSesion,
                Estado      = EstadoCierre.Abierto,
                UsuarioId   = command.UsuarioId
            };
        }
        else if (sesion.Estado == EstadoCierre.Cerrado)
        {
            throw new SesionDiariaCerradaException();
        }

        // FA4: validar comprobante duplicado (solo si se ingresan Número + Tipo)
        if (command.NumeroComprobante is not null && command.TipoComprobante.HasValue)
        {
            var duplicado = await _compraRepo.ComprobanteExisteAsync(
                command.ProveedorId, command.NumeroComprobante, command.TipoComprobante.Value, null, ct);
            if (duplicado)
                throw new ComprobanteDuplicadoException(command.NumeroComprobante, command.TipoComprobante.Value.ToString());
        }

        var ahora = DateTime.UtcNow;
        var items = new List<ItemDetalleCompra>(command.Items.Count);

        foreach (var inputItem in command.Items)
        {
            var producto = await _productoRepo.ObtenerPorIdAsync(inputItem.ProductoId, ct);
            if (producto is null || !producto.EstaActivo)
                throw new ProductoNoEncontradoException();

            // Resolver código y factor
            decimal factor          = 1m;
            Guid?   codigoId        = null;

            if (inputItem.CodigoProductoId.HasValue)
            {
                var codigo = producto.Codigos.FirstOrDefault(c => c.Id == inputItem.CodigoProductoId.Value);
                if (codigo is null)
                    throw new ProductoNoEncontradoException();

                factor   = codigo.Factor;
                codigoId = codigo.Id;
            }

            // Validar cantidad entera si la unidad es Unidad
            if (producto.UnidadMedida == UnidadMedida.Unidad && inputItem.Cantidad != Math.Floor(inputItem.Cantidad))
                throw new FluentValidation.ValidationException(
                    new[] { new FluentValidation.Results.ValidationFailure("Cantidad",
                        $"Para el producto '{producto.Nombre}' (unidad: Unidad), la cantidad debe ser entera.") });

            var cantidadEfectiva = inputItem.Cantidad * factor;
            var subtotal         = cantidadEfectiva * inputItem.PrecioCompra;

            var movimiento = new MovimientoStock
            {
                Tipo      = TipoMovimiento.Compra,
                Cantidad  = cantidadEfectiva,
                FechaHora = ahora,
                ProductoId = inputItem.ProductoId,
                UsuarioId = command.UsuarioId
            };

            var item = new ItemDetalleCompra
            {
                ProductoId      = inputItem.ProductoId,
                CodigoProductoId = codigoId,
                Cantidad        = cantidadEfectiva,
                PrecioCompra    = inputItem.PrecioCompra,
                Subtotal        = subtotal,
                NombreProducto  = producto.Nombre,
                Factor          = inputItem.CodigoProductoId.HasValue ? factor : null
            };
            item.Movimientos.Add(movimiento);
            items.Add(item);

            // Actualizar stock (el producto está tracked desde ObtenerPorIdAsync)
            producto.Stock.Cantidad            += cantidadEfectiva;
            producto.Stock.UltimaActualizacion  = ahora;

            // Actualizar precio de costo si el usuario lo aprobó
            if (inputItem.ActualizarPrecioCosto && inputItem.PrecioCompra != producto.PrecioCosto)
                producto.PrecioCosto = inputItem.PrecioCompra;
        }

        var totalCompra = items.Sum(i => i.Subtotal);

        var compra = new DetalleCompra
        {
            CompraDiaId       = 0, // EF lo resuelve por nav si la sesión es nueva
            ProveedorId       = command.ProveedorId,
            UsuarioId         = command.UsuarioId,
            FechaHora         = ahora,
            FechaCompra       = command.FechaCompra.Date,
            Total             = totalCompra,
            NumeroComprobante = command.NumeroComprobante,
            TipoComprobante   = command.TipoComprobante,
            FechaComprobante  = command.FechaComprobante,
            EstaAnulada       = false,
            Items             = items
        };

        // Vincular compra a la sesión
        compra.CompraDia = sesion;
        if (esNuevaSesion)
            sesion.Total += totalCompra;

        var compraId = await _compraRepo.RegistrarCompraAsync(sesion, esNuevaSesion, compra, totalCompra, ct);

        return new RegistrarCompraResponse(
            compra.Id,
            sesion.Id,
            proveedor.Id,
            proveedor.Nombre,
            compra.FechaCompra,
            compra.FechaHora,
            compra.Total,
            compra.NumeroComprobante,
            compra.TipoComprobante?.ToString(),
            compra.FechaComprobante,
            items.Select(i => new ItemCompraResponse(
                i.Id,
                i.ProductoId,
                i.NombreProducto,
                i.Cantidad,
                i.PrecioCompra,
                i.Subtotal,
                i.CodigoProductoId,
                i.Factor
            )).ToList()
        );
    }
}
