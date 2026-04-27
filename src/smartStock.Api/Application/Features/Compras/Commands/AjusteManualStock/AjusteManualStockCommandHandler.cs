using MediatR;
using smartStock.Api.Application.Common.Exceptions.Compras;
using smartStock.Api.Application.Common.Exceptions.Productos;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Enums;
using smartStock.Api.Domain.Models;
using smartStock.Shared.Dtos.Compras.AjusteManualStock;

namespace smartStock.Api.Application.Features.Compras.Commands.AjusteManualStock;

public sealed class AjusteManualStockCommandHandler
    : IRequestHandler<AjusteManualStockCommand, AjusteManualStockResponse>
{
    private readonly IProductoRepository _productoRepo;

    public AjusteManualStockCommandHandler(IProductoRepository productoRepo)
        => _productoRepo = productoRepo;

    public async Task<AjusteManualStockResponse> Handle(AjusteManualStockCommand command, CancellationToken ct)
    {
        var producto = await _productoRepo.ObtenerPorIdAsync(command.ProductoId, ct);
        if (producto is null || !producto.EstaActivo)
            throw new ProductoNoEncontradoException();

        // Validar unidad entera si aplica
        if (producto.UnidadMedida == UnidadMedida.Unidad && command.Cantidad != Math.Floor(command.Cantidad))
            throw new FluentValidation.ValidationException(
                new[] { new FluentValidation.Results.ValidationFailure("Cantidad",
                    $"Para el producto '{producto.Nombre}' (unidad: Unidad), la cantidad debe ser entera.") });

        var stockAnterior = producto.Stock.Cantidad;
        decimal cantidadDelta;

        if (command.TipoAjuste == TipoAjuste.Incremento)
        {
            cantidadDelta = command.Cantidad;
        }
        else
        {
            // FA2: stock insuficiente para decremento
            if (command.Cantidad > stockAnterior)
                throw new StockInsuficienteParaDecremento(stockAnterior);

            cantidadDelta = -command.Cantidad;
        }

        var ahora = DateTime.UtcNow;

        producto.Stock.Cantidad           += cantidadDelta;
        producto.Stock.UltimaActualizacion = ahora;

        var movimiento = new MovimientoStock
        {
            Tipo        = TipoMovimiento.Ajuste,
            Cantidad    = cantidadDelta,
            FechaHora   = ahora,
            Observacion = command.Motivo,
            ProductoId  = command.ProductoId,
            UsuarioId   = command.UsuarioId
        };

        await _productoRepo.AjustarStockAsync(movimiento, ct);

        return new AjusteManualStockResponse(
            movimiento.Id,
            producto.Id,
            producto.Nombre,
            command.TipoAjuste.ToString(),
            command.Cantidad,
            stockAnterior,
            producto.Stock.Cantidad,
            command.Motivo,
            ahora
        );
    }
}
