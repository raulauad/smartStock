using MediatR;
using smartStock.Shared.Dtos.Admin.EditarProveedor;
using smartStock.Shared.Dtos.Shared;

namespace smartStock.Api.Application.Features.Admin.Commands.EditarProveedor;

public sealed record EditarProveedorCommand(
    string       Nombre,
    string?      Cuit,
    string       Telefono,
    string       Email,
    DireccionDto Direccion,
    string?      Observaciones
) : IRequest<EditarProveedorResponse>
{
    // Asignado por el controller desde el parámetro de ruta {id} — nunca del body.
    public Guid ProveedorId { get; init; }
}
