using MediatR;
using smartStock.Api.Application.Common.Exceptions.Categorias;
using smartStock.Api.Application.Common.Exceptions.Productos;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Shared.Dtos.Admin.EditarProducto;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Commands.EditarProducto;

public sealed class EditarProductoCommandHandler
    : IRequestHandler<EditarProductoCommand, EditarProductoResponse>
{
    private readonly IProductoRepository  _productoRepo;
    private readonly ICategoriaRepository _categoriaRepo;

    public EditarProductoCommandHandler(
        IProductoRepository  productoRepo,
        ICategoriaRepository categoriaRepo)
    {
        _productoRepo  = productoRepo;
        _categoriaRepo = categoriaRepo;
    }

    public async Task<EditarProductoResponse> Handle(
        EditarProductoCommand command,
        CancellationToken     cancellationToken)
    {
        var producto = await _productoRepo.ObtenerPorIdAsync(command.ProductoId, cancellationToken)
            ?? throw new ProductoNoEncontradoException();

        // Verificar categoría activa
        var categoria = await _categoriaRepo.ObtenerPorIdAsync(command.CategoriaId, cancellationToken);
        if (categoria is null || !categoria.EstaActivo)
            throw new CategoriaNoEncontradaException();

        // FA3: cambio de unidad de medida con stock existente
        if (producto.UnidadMedida != command.UnidadMedida && producto.Stock.Cantidad != 0)
            throw new UnidadMedidaConStockException();

        // FA2: precio de venta menor al costo
        if (command.PrecioVenta < command.PrecioCosto && !command.ConfirmarPrecioVentaMenorCosto)
            throw new PrecioVentaMenorCostoException();

        // FA4: nombre similar a otro producto activo
        var nombreLower = command.Nombre.Trim();
        var nombresSimilares = await _productoRepo.ObtenerNombresSimilaresAsync(
            nombreLower, command.ProductoId, cancellationToken);
        if (nombresSimilares.Count > 0 && !command.ConfirmarNombreSimilar)
            throw new NombreSimilarProductoException(nombresSimilares);

        producto.Nombre       = nombreLower;
        producto.CategoriaId  = command.CategoriaId;
        producto.UnidadMedida = command.UnidadMedida;
        producto.PrecioCosto  = command.PrecioCosto;
        producto.PrecioVenta  = command.PrecioVenta;
        producto.StockMinimo  = command.StockMinimo;

        await _productoRepo.ActualizarAsync(producto, cancellationToken);

        return new EditarProductoResponse(
            producto.Id,
            producto.Nombre,
            categoria.Nombre,
            producto.UnidadMedida.ToString(),
            producto.PrecioCosto,
            producto.PrecioVenta,
            producto.StockMinimo
        );
    }
}
