using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Application.Common.Exceptions.Categorias;
using smartStock.Api.Application.Common.Exceptions.Productos;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Enums;
using smartStock.Api.Domain.Models;
using smartStock.Shared.Dtos.Admin.AltaProducto;
using smartStock.Shared.Dtos.Admin.Productos;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Commands.AltaProducto;

public sealed class AltaProductoCommandHandler
    : IRequestHandler<AltaProductoCommand, AltaProductoResponse>
{
    private readonly IProductoRepository  _productoRepo;
    private readonly ICategoriaRepository _categoriaRepo;

    public AltaProductoCommandHandler(
        IProductoRepository  productoRepo,
        ICategoriaRepository categoriaRepo)
    {
        _productoRepo  = productoRepo;
        _categoriaRepo = categoriaRepo;
    }

    public async Task<AltaProductoResponse> Handle(
        AltaProductoCommand command,
        CancellationToken   cancellationToken)
    {
        var nombre = command.Nombre.Trim();

        // Verificar categoría activa
        var categoria = await _categoriaRepo.ObtenerPorIdAsync(command.CategoriaId, cancellationToken);
        if (categoria is null || !categoria.EstaActivo)
            throw new CategoriaNoEncontradaException();

        // FA3: precio de venta menor al costo
        if (command.PrecioVenta < command.PrecioCosto && !command.ConfirmarPrecioVentaMenorCosto)
            throw new PrecioVentaMenorCostoException();

        // FA4: nombre similar a producto activo
        var nombresSimilares = await _productoRepo.ObtenerNombresSimilaresAsync(nombre, null, cancellationToken);
        if (nombresSimilares.Count > 0 && !command.ConfirmarNombreSimilar)
            throw new NombreSimilarProductoException(nombresSimilares);

        // Preparar códigos
        List<CodigoProducto> codigos;

        if (command.Codigos is { Count: > 0 })
        {
            // Validar unicidad de cada código ingresado
            foreach (var input in command.Codigos)
            {
                if (await _productoRepo.CodigoExisteAsync(input.Codigo, null, cancellationToken))
                    throw new CodigoDuplicadoException(input.Codigo);
            }

            codigos = command.Codigos
                .Select(c => new CodigoProducto
                {
                    Id          = Guid.NewGuid(),
                    Codigo      = c.Codigo.Trim(),
                    Tipo        = c.Tipo,
                    Factor      = c.Factor,
                    Descripcion = c.Descripcion?.Trim()
                })
                .ToList();
        }
        else
        {
            // FA5 de códigos: autogenerar código interno
            var codigoAuto = await _productoRepo.GenerarCodigoInternoAsync(cancellationToken);
            codigos =
            [
                new CodigoProducto
                {
                    Id          = Guid.NewGuid(),
                    Codigo      = codigoAuto,
                    Tipo        = TipoCodigo.Interno,
                    Factor      = 1,
                    Descripcion = "Código interno autogenerado"
                }
            ];
        }

        var ahora = DateTime.UtcNow;

        // Crear movimiento de alta si hay stock inicial
        List<MovimientoStock> movimientos = [];
        if (command.StockInicial > 0)
        {
            movimientos.Add(new MovimientoStock
            {
                Tipo      = TipoMovimiento.AltaInicial,
                Cantidad  = command.StockInicial,
                FechaHora = ahora,
                UsuarioId = command.UsuarioAltaId
            });
        }

        var producto = new Producto
        {
            Id           = Guid.NewGuid(),
            Nombre       = nombre,
            CategoriaId  = command.CategoriaId,
            UnidadMedida = command.UnidadMedida,
            PrecioCosto  = command.PrecioCosto,
            PrecioVenta  = command.PrecioVenta,
            StockMinimo  = command.StockMinimo,
            EstaActivo   = true,
            FechaAlta    = ahora,
            UsuarioAltaId = command.UsuarioAltaId,
            Stock = new StockActual
            {
                Cantidad            = command.StockInicial,
                UltimaActualizacion = ahora
            },
            Codigos     = codigos,
            Movimientos = movimientos
        };

        try
        {
            await _productoRepo.CrearAsync(producto, cancellationToken);
        }
        catch (DbUpdateException)
        {
            // Revalidar en caso de race condition en código único
            foreach (var cod in producto.Codigos)
            {
                if (await _productoRepo.CodigoExisteAsync(cod.Codigo, producto.Id, cancellationToken))
                    throw new CodigoDuplicadoException(cod.Codigo);
            }
            throw;
        }

        return new AltaProductoResponse(
            producto.Id,
            producto.Nombre,
            categoria.Nombre,
            producto.UnidadMedida.ToString(),
            producto.PrecioCosto,
            producto.PrecioVenta,
            producto.Stock.Cantidad,
            producto.StockMinimo,
            producto.EstaActivo,
            producto.FechaAlta,
            producto.Codigos
                .Select(c => new CodigoProductoResponse(c.Id, c.Codigo, c.Tipo.ToString(), c.Factor, c.Descripcion))
                .ToList()
        );
    }
}
