using MediatR;
using smartStock.Shared.Dtos.Admin.AltaCategoria;

namespace smartStock.Api.Application.Features.Admin.Commands.AltaCategoria;

public sealed record AltaCategoriaCommand(
    string  Nombre,
    string? Descripcion
) : IRequest<AltaCategoriaResponse>
{
    // Asignado por el controller desde el claim sub del JWT — nunca del body.
    public Guid UsuarioAltaId { get; init; }
}
