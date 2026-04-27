using smartStock.Api.Domain.Enums;
using smartStock.Shared.Dtos.Compras.RegistrarCompra;
using MediatR;

namespace smartStock.Api.Application.Features.Compras.Commands.RegistrarCompra;

public sealed record RegistrarCompraCommand(
    Guid                   ProveedorId,
    DateTime               FechaCompra,
    string?                NumeroComprobante,
    TipoComprobante?       TipoComprobante,
    DateTime?              FechaComprobante,
    List<ItemCompraInput>  Items,
    bool                   ConfirmarFechaRetroactiva
) : IRequest<RegistrarCompraResponse>
{
    public Guid UsuarioId { get; init; }
}

public sealed record ItemCompraInput(
    Guid     ProductoId,
    Guid?    CodigoProductoId,
    decimal  Cantidad,
    decimal  PrecioCompra,
    bool     ActualizarPrecioCosto
);
