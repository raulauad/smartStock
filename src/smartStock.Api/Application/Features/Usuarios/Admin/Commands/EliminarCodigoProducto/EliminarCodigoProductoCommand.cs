using MediatR;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Commands.EliminarCodigoProducto;

public sealed record EliminarCodigoProductoCommand : IRequest
{
    // Asignados por el controller desde la ruta — nunca del body.
    public Guid ProductoId { get; init; }
    public Guid CodigoId   { get; init; }
}
