using MediatR;
using smartStock.Api.Application.Common.Exceptions.Ventas;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Enums;
using smartStock.Api.Domain.Models;
using smartStock.Shared.Dtos.Ventas.AnularVenta;

namespace smartStock.Api.Application.Features.Ventas.Commands.AnularVenta;

public sealed class AnularVentaCommandHandler
    : IRequestHandler<AnularVentaCommand, AnularVentaResponse>
{
    private readonly IVentaRepository    _ventaRepo;
    private readonly IProductoRepository _productoRepo;
    private readonly IUsuarioRepository  _usuarioRepo;

    public AnularVentaCommandHandler(
        IVentaRepository    ventaRepo,
        IProductoRepository productoRepo,
        IUsuarioRepository  usuarioRepo)
    {
        _ventaRepo    = ventaRepo;
        _productoRepo = productoRepo;
        _usuarioRepo  = usuarioRepo;
    }

    public async Task<AnularVentaResponse> Handle(AnularVentaCommand command, CancellationToken ct)
    {
        var venta = await _ventaRepo.ObtenerVentaPorIdConItemsAsync(command.VentaId, ct);
        if (venta is null)
            throw new VentaNoEncontradaException();

        // FA2: ya anulada
        if (venta.EstaAnulada)
            throw new VentaYaAnuladaException();

        // FA1: sesión cerrada
        if (venta.VentaDia.Estado == EstadoCierre.Cerrado)
            throw new SesionDiariaVentaCerradaException();

        var ahora = DateTime.UtcNow;

        // Cargar productos para devolver stock (EF identity map evita cargas duplicadas)
        var productosDict = new Dictionary<Guid, Producto>();
        foreach (var item in venta.Items)
        {
            if (productosDict.ContainsKey(item.ProductoId)) continue;
            var producto = await _productoRepo.ObtenerPorIdAsync(item.ProductoId, ct);
            if (producto is not null)
                productosDict[item.ProductoId] = producto;
        }

        // Devolver stock y crear movimientos compensatorios (no puede fallar: siempre se suma)
        foreach (var item in venta.Items)
        {
            if (!productosDict.TryGetValue(item.ProductoId, out var producto)) continue;

            var movComp = new MovimientoStock
            {
                Tipo        = TipoMovimiento.Anulacion,
                Cantidad    = item.Cantidad,    // positivo: devuelve stock
                FechaHora   = ahora,
                Observacion = $"Anulación de venta #{venta.Id}: {command.MotivoAnulacion}",
                ProductoId  = item.ProductoId,
                UsuarioId   = command.UsuarioId,
                ItemVentaId = item.Id
            };
            item.Movimientos.Add(movComp);

            producto.Stock.Cantidad           += item.Cantidad;
            producto.Stock.UltimaActualizacion = ahora;
        }

        venta.EstaAnulada     = true;
        venta.FechaAnulacion  = ahora;
        venta.UsuarioAnulaId  = command.UsuarioId;
        venta.MotivoAnulacion = command.MotivoAnulacion;

        // Recalcular total de la sesión
        venta.VentaDia.Total -= venta.Total;

        await _ventaRepo.GuardarCambiosAsync(ct);

        var usuarioAnula = await _usuarioRepo.ObtenerPorIdAsync(command.UsuarioId, ct);

        return new AnularVentaResponse(
            venta.Id,
            true,
            ahora,
            command.MotivoAnulacion,
            usuarioAnula?.Nombre ?? string.Empty
        );
    }
}
