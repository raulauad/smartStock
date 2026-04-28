using MediatR;
using smartStock.Shared.Dtos.Admin.ObtenerListaProveedores;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Queries.ObtenerListaProveedores;

public sealed record ObtenerListaProveedoresQuery(
    string? FiltroEstado,
    string? Busqueda
) : IRequest<IReadOnlyList<ObtenerListaProveedoresResponse>>;
