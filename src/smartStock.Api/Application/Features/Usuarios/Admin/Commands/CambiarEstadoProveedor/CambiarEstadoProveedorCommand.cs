using MediatR;
using smartStock.Shared.Dtos.Admin.CambiarEstadoProveedor;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Commands.CambiarEstadoProveedor;

public sealed record CambiarEstadoProveedorCommand(
    bool EstaActivo
) : IRequest<CambiarEstadoProveedorResponse>
{
    // Asignado por el controller desde el parámetro de ruta {id} — nunca del body.
    public Guid ProveedorId { get; init; }
}
