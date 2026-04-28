using MediatR;
using smartStock.Shared.Dtos.Admin.CambiarEstadoCategoria;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Commands.CambiarEstadoCategoria;

public sealed record CambiarEstadoCategoriaCommand(
    bool  EstaActivo,
    Guid? CategoriaDestinoId
) : IRequest<CambiarEstadoCategoriaResponse>
{
    // Asignado por el controller desde el parámetro de ruta {id} — nunca del body.
    public Guid CategoriaId { get; init; }
}
