using FluentValidation;
using FluentValidation.Results;
using MediatR;
using smartStock.Api.Application.Common.Exceptions.Productos;
using smartStock.Api.Application.Common.Exceptions.Ventas;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Enums;
using smartStock.Api.Domain.Models;
using smartStock.Shared.Dtos.Ventas.RegistrarVenta;

namespace smartStock.Api.Application.Features.Ventas.Commands.RegistrarVenta;

public sealed class RegistrarVentaCommandHandler
    : IRequestHandler<RegistrarVentaCommand, RegistrarVentaResponse>
{
    private readonly IVentaRepository    _ventaRepo;
    private readonly IProductoRepository _productoRepo;

    public RegistrarVentaCommandHandler(
        IVentaRepository    ventaRepo,
        IProductoRepository productoRepo)
    {
        _ventaRepo    = ventaRepo;
        _productoRepo = productoRepo;
    }

    public async Task<RegistrarVentaResponse> Handle(RegistrarVentaCommand command, CancellationToken ct)
    {
        var ahora       = DateTime.UtcNow;
        var fechaSesion = ahora.Date;

        // Obtener o crear sesión diaria
        var sesion      = await _ventaRepo.ObtenerSesionDiariaPorFechaAsync(fechaSesion, ct);
        var esNuevaSesion = sesion is null;

        if (sesion is null)
        {
            sesion = new VentaDia
            {
                FechaSesion = fechaSesion,
                Estado      = EstadoCierre.Abierto,
                UsuarioId   = command.UsuarioId
            };
        }
        else if (sesion.Estado == EstadoCierre.Cerrado)
        {
            throw new SesionDiariaVentaCerradaException();
        }

        var items        = new List<ItemDetalleVenta>(command.Items.Count);
        var stockUpdates = new List<(Guid, string, decimal)>(command.Items.Count);

        foreach (var inputItem in command.Items)
        {
            var producto = await _productoRepo.ObtenerPorIdAsync(inputItem.ProductoId, ct);
            if (producto is null || !producto.EstaActivo)
                throw new ProductoNoEncontradoException();

            decimal factor   = 1m;
            Guid?   codigoId = null;

            if (inputItem.CodigoProductoId.HasValue)
            {
                var codigo = producto.Codigos.FirstOrDefault(c => c.Id == inputItem.CodigoProductoId.Value);
                if (codigo is null)
                    throw new ProductoNoEncontradoException();

                factor   = codigo.Factor;
                codigoId = codigo.Id;
            }

            // Cantidad entera requerida para productos en Unidades
            if (producto.UnidadMedida == UnidadMedida.Unidad && inputItem.Cantidad != Math.Floor(inputItem.Cantidad))
                throw new ValidationException(new[]
                {
                    new ValidationFailure("Cantidad",
                        $"Para el producto '{producto.Nombre}' (unidad: Unidad), la cantidad debe ser entera.")
                });

            var cantidadEfectiva = inputItem.Cantidad * factor;
            var subtotal         = cantidadEfectiva * producto.PrecioVenta;
            var ganancia         = (producto.PrecioVenta - producto.PrecioCosto) * cantidadEfectiva;

            var movimiento = new MovimientoStock
            {
                Tipo      = TipoMovimiento.Venta,
                Cantidad  = -cantidadEfectiva,   // egreso de stock
                FechaHora = ahora,
                ProductoId = inputItem.ProductoId,
                UsuarioId  = command.UsuarioId
            };

            var item = new ItemDetalleVenta
            {
                ProductoId       = inputItem.ProductoId,
                CodigoProductoId = codigoId,
                Cantidad         = cantidadEfectiva,
                PrecioVenta      = producto.PrecioVenta,
                PrecioCosto      = producto.PrecioCosto,
                Subtotal         = subtotal,
                GananciaTotal    = ganancia,
                NombreProducto   = producto.Nombre,
                Factor           = inputItem.CodigoProductoId.HasValue ? factor : null
            };
            item.Movimientos.Add(movimiento);
            items.Add(item);

            stockUpdates.Add((inputItem.ProductoId, producto.Nombre, cantidadEfectiva));
        }

        var totalVenta = items.Sum(i => i.Subtotal);

        // FA5: MontoRecibido menor al total solo aplica para Efectivo
        if (command.FormaPago == FormaPago.Efectivo &&
            command.MontoRecibido.HasValue &&
            command.MontoRecibido.Value < totalVenta)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure("MontoRecibido",
                    $"El monto recibido ({command.MontoRecibido.Value:F2}) es menor al total de la venta ({totalVenta:F2}).")
            });
        }

        var venta = new DetalleVenta
        {
            VentaDia      = sesion,
            UsuarioId     = command.UsuarioId,
            FechaHora     = ahora,
            Total         = totalVenta,
            FormaPago     = command.FormaPago,
            MontoRecibido = command.FormaPago == FormaPago.Efectivo ? command.MontoRecibido : null,
            EstaAnulada   = false,
            Items         = items
        };

        await _ventaRepo.RegistrarVentaAsync(
            sesion, esNuevaSesion, venta, stockUpdates, ahora, ct);

        return new RegistrarVentaResponse(
            venta.Id,
            sesion.Id,
            venta.NumeroComprobante,
            venta.FechaHora,
            venta.Total,
            venta.FormaPago.ToString(),
            venta.MontoRecibido,
            items.Select(i => new ItemVentaResponse(
                i.Id,
                i.ProductoId,
                i.NombreProducto,
                i.Cantidad,
                i.PrecioVenta,
                i.Subtotal,
                i.CodigoProductoId,
                i.Factor
            )).ToList()
        );
    }
}
