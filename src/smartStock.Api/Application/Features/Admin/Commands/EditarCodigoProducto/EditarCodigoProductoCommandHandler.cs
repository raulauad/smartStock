using MediatR;
using smartStock.Api.Application.Common.Exceptions.Productos;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Shared.Dtos.Admin.EditarCodigoProducto;

namespace smartStock.Api.Application.Features.Admin.Commands.EditarCodigoProducto;

public sealed class EditarCodigoProductoCommandHandler
    : IRequestHandler<EditarCodigoProductoCommand, EditarCodigoProductoResponse>
{
    private readonly IProductoRepository _productoRepo;

    public EditarCodigoProductoCommandHandler(IProductoRepository productoRepo)
        => _productoRepo = productoRepo;

    public async Task<EditarCodigoProductoResponse> Handle(
        EditarCodigoProductoCommand command,
        CancellationToken           cancellationToken)
    {
        // Verificar que el producto existe
        var existeProducto = await _productoRepo.ObtenerPorIdAsync(command.ProductoId, cancellationToken);
        if (existeProducto is null)
            throw new ProductoNoEncontradoException();

        var codigo = await _productoRepo.ObtenerCodigoPorIdAsync(command.CodigoId, command.ProductoId, cancellationToken)
            ?? throw new CodigoNoEncontradoException();

        codigo.Factor      = command.Factor;
        codigo.Descripcion = command.Descripcion?.Trim();

        await _productoRepo.ActualizarCodigoAsync(codigo, cancellationToken);

        return new EditarCodigoProductoResponse(
            codigo.Id,
            codigo.Codigo,
            codigo.Tipo.ToString(),
            codigo.Factor,
            codigo.Descripcion
        );
    }
}
