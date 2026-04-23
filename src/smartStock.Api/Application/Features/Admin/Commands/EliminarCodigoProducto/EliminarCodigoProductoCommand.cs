using MediatR;

namespace smartStock.Api.Application.Features.Admin.Commands.EliminarCodigoProducto;

public sealed record EliminarCodigoProductoCommand : IRequest
{
    // Asignados por el controller desde la ruta — nunca del body.
    public Guid ProductoId { get; init; }
    public Guid CodigoId   { get; init; }
}
