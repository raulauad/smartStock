using MediatR;
using smartStock.Api.Application.Common.Exceptions.Productos;
using smartStock.Api.Application.Common.Interfaces.Repositories;

namespace smartStock.Api.Application.Features.Admin.Commands.EliminarCodigoProducto;

public sealed class EliminarCodigoProductoCommandHandler : IRequestHandler<EliminarCodigoProductoCommand>
{
    private readonly IProductoRepository _productoRepo;

    public EliminarCodigoProductoCommandHandler(IProductoRepository productoRepo)
        => _productoRepo = productoRepo;

    public async Task Handle(
        EliminarCodigoProductoCommand command,
        CancellationToken             cancellationToken)
    {
        // Verificar que el producto existe
        var existeProducto = await _productoRepo.ObtenerPorIdAsync(command.ProductoId, cancellationToken);
        if (existeProducto is null)
            throw new ProductoNoEncontradoException();

        var codigo = await _productoRepo.ObtenerCodigoPorIdAsync(command.CodigoId, command.ProductoId, cancellationToken)
            ?? throw new CodigoNoEncontradoException();

        // FA2: no permitir eliminar el único código del producto
        var totalCodigos = await _productoRepo.ContarCodigosAsync(command.ProductoId, cancellationToken);
        if (totalCodigos <= 1)
            throw new CodigoUnicoRequeridoException();

        await _productoRepo.EliminarCodigoAsync(codigo, cancellationToken);
    }
}
