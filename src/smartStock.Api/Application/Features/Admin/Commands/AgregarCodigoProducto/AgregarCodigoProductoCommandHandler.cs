using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Application.Common.Exceptions.Productos;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Models;
using smartStock.Shared.Dtos.Admin.AgregarCodigoProducto;
using smartStock.Shared.Dtos.Admin.Productos;

namespace smartStock.Api.Application.Features.Admin.Commands.AgregarCodigoProducto;

public sealed class AgregarCodigoProductoCommandHandler
    : IRequestHandler<AgregarCodigoProductoCommand, AgregarCodigoProductoResponse>
{
    private readonly IProductoRepository _productoRepo;

    public AgregarCodigoProductoCommandHandler(IProductoRepository productoRepo)
        => _productoRepo = productoRepo;

    public async Task<AgregarCodigoProductoResponse> Handle(
        AgregarCodigoProductoCommand command,
        CancellationToken            cancellationToken)
    {
        var producto = await _productoRepo.ObtenerPorIdAsync(command.ProductoId, cancellationToken)
            ?? throw new ProductoNoEncontradoException();

        var codigo = command.Codigo.Trim();

        if (await _productoRepo.CodigoExisteAsync(codigo, command.ProductoId, cancellationToken))
            throw new CodigoDuplicadoException(codigo);

        var nuevoCodigo = new CodigoProducto
        {
            Id          = Guid.NewGuid(),
            Codigo      = codigo,
            Tipo        = command.Tipo,
            Factor      = command.Factor,
            Descripcion = command.Descripcion?.Trim(),
            ProductoId  = command.ProductoId
        };

        try
        {
            await _productoRepo.AgregarCodigoAsync(nuevoCodigo, cancellationToken);
        }
        catch (DbUpdateException)
        {
            if (await _productoRepo.CodigoExisteAsync(codigo, command.ProductoId, cancellationToken))
                throw new CodigoDuplicadoException(codigo);
            throw;
        }

        // Recargar producto para devolver lista actualizada
        var productoActualizado = await _productoRepo.ObtenerPorIdAsync(command.ProductoId, cancellationToken)!;

        return new AgregarCodigoProductoResponse(
            producto.Id,
            producto.Nombre,
            productoActualizado!.Codigos
                .Select(c => new CodigoProductoResponse(c.Id, c.Codigo, c.Tipo.ToString(), c.Factor, c.Descripcion))
                .ToList()
        );
    }
}
