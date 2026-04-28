using MediatR;
using smartStock.Shared.Dtos.Admin.AltaProveedor;
using smartStock.Shared.Dtos.Shared;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Commands.AltaProveedor;

public sealed record AltaProveedorCommand(
    string       Nombre,
    string?      Cuit,
    string       Telefono,
    string       Email,
    DireccionDto Direccion,
    string?      Observaciones
) : IRequest<AltaProveedorResponse>
{
    // Asignado por el controller desde el claim sub del JWT — nunca del body.
    public Guid UsuarioAltaId { get; init; }
}
