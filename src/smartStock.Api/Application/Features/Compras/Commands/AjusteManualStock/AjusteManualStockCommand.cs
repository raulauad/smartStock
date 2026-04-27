using MediatR;
using smartStock.Api.Domain.Enums;
using smartStock.Shared.Dtos.Compras.AjusteManualStock;

namespace smartStock.Api.Application.Features.Compras.Commands.AjusteManualStock;

public sealed record AjusteManualStockCommand(
    Guid       ProductoId,
    TipoAjuste TipoAjuste,
    decimal    Cantidad,
    string     Motivo
) : IRequest<AjusteManualStockResponse>
{
    public Guid UsuarioId { get; init; }
}
