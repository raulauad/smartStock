using MediatR;
using smartStock.Shared.Dtos.Compras.ObtenerAjustesStock;

namespace smartStock.Api.Application.Features.Compras.Queries.ObtenerAjustesStock;

public sealed record ObtenerAjustesStockQuery(
    DateTime? FechaDesde,
    DateTime? FechaHasta,
    Guid?     ProductoId,
    Guid?     UsuarioId,
    string?   FiltroTipo   // "ajuste" | "anulacion" | null (ambos)
) : IRequest<List<ObtenerAjustesStockResponse>>;
