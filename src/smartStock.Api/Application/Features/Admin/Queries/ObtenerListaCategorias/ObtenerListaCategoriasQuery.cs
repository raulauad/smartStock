using MediatR;
using smartStock.Shared.Dtos.Admin.ObtenerListaCategorias;

namespace smartStock.Api.Application.Features.Admin.Queries.ObtenerListaCategorias;

public sealed record ObtenerListaCategoriasQuery(
    string? FiltroEstado,
    string? Busqueda
) : IRequest<IReadOnlyList<ObtenerListaCategoriasResponse>>;
