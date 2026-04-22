using MediatR;
using smartStock.Shared.Dtos.Admin.EditarCategoria;

namespace smartStock.Api.Application.Features.Admin.Commands.EditarCategoria;

public sealed record EditarCategoriaCommand(
    string  Nombre,
    string? Descripcion
) : IRequest<EditarCategoriaResponse>
{
    // Asignado por el controller desde el parámetro de ruta {id} — nunca del body.
    public Guid CategoriaId { get; init; }
}
