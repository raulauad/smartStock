using MediatR;
using smartStock.Api.Application.Common.Exceptions.Compras;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Enums;
using smartStock.Api.Domain.Models;
using smartStock.Shared.Dtos.Compras.AnularCompra;

namespace smartStock.Api.Application.Features.Compras.Commands.AnularCompra;

public sealed class AnularCompraCommandHandler
    : IRequestHandler<AnularCompraCommand, AnularCompraResponse>
{
    private readonly ICompraRepository   _compraRepo;
    private readonly IProductoRepository _productoRepo;
    private readonly IUsuarioRepository  _usuarioRepo;

    public AnularCompraCommandHandler(
        ICompraRepository   compraRepo,
        IProductoRepository productoRepo,
        IUsuarioRepository  usuarioRepo)
    {
        _compraRepo   = compraRepo;
        _productoRepo = productoRepo;
        _usuarioRepo  = usuarioRepo;
    }

    public async Task<AnularCompraResponse> Handle(AnularCompraCommand command, CancellationToken ct)
    {
        var compra = await _compraRepo.ObtenerCompraPorIdConItemsAsync(command.CompraId, ct);
        if (compra is null)
            throw new CompraNoEncontradaException();

        // FA3: ya anulada
        if (compra.EstaAnulada)
            throw new CompraYaAnuladaException();

        // FA1: sesión cerrada
        if (compra.CompraDia.Estado == EstadoCierre.Cerrado)
            throw new SesionDiariaCerradaException();

        var ahora = DateTime.UtcNow;

        // Cargar todos los productos involucrados una sola vez (EF identity map evita duplicados)
        var productosDict = new Dictionary<Guid, Domain.Models.Producto>();
        foreach (var item in compra.Items)
        {
            if (productosDict.ContainsKey(item.ProductoId)) continue;
            var producto = await _productoRepo.ObtenerPorIdAsync(item.ProductoId, ct);
            if (producto is not null)
                productosDict[item.ProductoId] = producto;
        }

        // FA2: verificar stock suficiente acumulando por producto
        // (un mismo producto puede aparecer en varios ítems de la misma compra)
        var cantidadTotal = compra.Items
            .GroupBy(i => i.ProductoId)
            .ToDictionary(g => g.Key, g => g.Sum(i => i.Cantidad));

        foreach (var (productoId, cantidadRequerida) in cantidadTotal)
        {
            if (!productosDict.TryGetValue(productoId, out var prod)) continue;
            if (prod.Stock.Cantidad < cantidadRequerida)
            {
                var nombre = compra.Items.First(i => i.ProductoId == productoId).NombreProducto;
                throw new StockInsuficienteParaAnulacionException(nombre, prod.Stock.Cantidad, cantidadRequerida);
            }
        }

        // Aplicar reversión: decrementar stock y crear movimientos compensatorios
        foreach (var item in compra.Items)
        {
            if (!productosDict.TryGetValue(item.ProductoId, out var producto)) continue;

            var movComp = new MovimientoStock
            {
                Tipo         = TipoMovimiento.Anulacion,
                Cantidad     = -item.Cantidad,
                FechaHora    = ahora,
                Observacion  = $"Anulación de compra #{compra.Id}: {command.MotivoAnulacion}",
                ProductoId   = item.ProductoId,
                UsuarioId    = command.UsuarioId,
                ItemCompraId = item.Id
            };
            item.Movimientos.Add(movComp);

            producto.Stock.Cantidad           -= item.Cantidad;
            producto.Stock.UltimaActualizacion = ahora;
        }

        // Marcar compra como anulada
        compra.EstaAnulada     = true;
        compra.FechaAnulacion  = ahora;
        compra.UsuarioAnulaId  = command.UsuarioId;
        compra.MotivoAnulacion = command.MotivoAnulacion;

        // Recalcular total de la sesión
        compra.CompraDia.Total -= compra.Total;

        await _compraRepo.GuardarCambiosAsync(ct);

        var usuarioAnula = await _usuarioRepo.ObtenerPorIdAsync(command.UsuarioId, ct);

        return new AnularCompraResponse(
            compra.Id,
            true,
            ahora,
            command.MotivoAnulacion,
            usuarioAnula?.Nombre ?? string.Empty
        );
    }
}
