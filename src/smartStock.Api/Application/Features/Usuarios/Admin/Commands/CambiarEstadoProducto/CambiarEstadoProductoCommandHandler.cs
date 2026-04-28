using MediatR;
using smartStock.Api.Application.Common.Exceptions.Productos;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Shared.Dtos.Admin.CambiarEstadoProducto;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Commands.CambiarEstadoProducto;

public sealed class CambiarEstadoProductoCommandHandler
    : IRequestHandler<CambiarEstadoProductoCommand, CambiarEstadoProductoResponse>
{
    private readonly IProductoRepository _productoRepo;

    public CambiarEstadoProductoCommandHandler(IProductoRepository productoRepo)
        => _productoRepo = productoRepo;

    public async Task<CambiarEstadoProductoResponse> Handle(
        CambiarEstadoProductoCommand command,
        CancellationToken            cancellationToken)
    {
        var producto = await _productoRepo.ObtenerPorIdAsync(command.ProductoId, cancellationToken)
            ?? throw new ProductoNoEncontradoException();

        if (producto.EstaActivo == command.EstaActivo)
            throw new EstadoProductoSinCambioException();

        producto.EstaActivo = command.EstaActivo;

        await _productoRepo.ActualizarAsync(producto, cancellationToken);

        return new CambiarEstadoProductoResponse(
            producto.Id,
            producto.Nombre,
            producto.EstaActivo,
            producto.Stock.Cantidad
        );
    }
}
