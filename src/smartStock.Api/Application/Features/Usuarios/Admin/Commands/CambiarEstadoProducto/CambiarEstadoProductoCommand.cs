using MediatR;
using smartStock.Shared.Dtos.Admin.CambiarEstadoProducto;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Commands.CambiarEstadoProducto;

public sealed record CambiarEstadoProductoCommand(
    bool EstaActivo
) : IRequest<CambiarEstadoProductoResponse>
{
    // Asignado por el controller desde la ruta — nunca del body.
    public Guid ProductoId { get; init; }
}
