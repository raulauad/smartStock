using MediatR;
using smartStock.Shared.Dtos.Admin.EditarCodigoProducto;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Commands.EditarCodigoProducto;

public sealed record EditarCodigoProductoCommand(
    decimal  Factor,
    string?  Descripcion
) : IRequest<EditarCodigoProductoResponse>
{
    // Asignados por el controller desde la ruta — nunca del body.
    public Guid ProductoId { get; init; }
    public Guid CodigoId   { get; init; }
}
