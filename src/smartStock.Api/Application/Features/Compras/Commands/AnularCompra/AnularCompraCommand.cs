using MediatR;
using smartStock.Shared.Dtos.Compras.AnularCompra;

namespace smartStock.Api.Application.Features.Compras.Commands.AnularCompra;

public sealed record AnularCompraCommand(
    string MotivoAnulacion
) : IRequest<AnularCompraResponse>
{
    public int  CompraId  { get; init; }
    public Guid UsuarioId { get; init; }
}
