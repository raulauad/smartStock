using MediatR;
using smartStock.Shared.Dtos.Admin.ObtenerListaProductos;

namespace smartStock.Api.Application.Features.Admin.Queries.ObtenerListaProductos;

public sealed record ObtenerListaProductosQuery(
    string? FiltroEstado,
    Guid?   FiltroCategoria,
    bool?   AlertaStockBajo,
    string? Busqueda
) : IRequest<List<ObtenerListaProductosResponse>>;
