using MediatR;
using smartStock.Api.Domain.Enums;
using smartStock.Shared.Dtos.Ventas.RegistrarVenta;

namespace smartStock.Api.Application.Features.Ventas.Commands.RegistrarVenta;

public sealed record RegistrarVentaCommand : IRequest<RegistrarVentaResponse>
{
    public Guid                 UsuarioId     { get; init; }   // inyectado desde JWT en controller
    public List<ItemVentaInput> Items         { get; init; } = [];
    public FormaPago            FormaPago     { get; init; }
    public decimal?             MontoRecibido { get; init; }
}

public sealed record ItemVentaInput(
    Guid    ProductoId,
    Guid?   CodigoProductoId,
    decimal Cantidad
);
