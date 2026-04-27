using MediatR;
using smartStock.Shared.Dtos.Compras.ObtenerListaCompras;

namespace smartStock.Api.Application.Features.Compras.Queries.ObtenerListaCompras;

public sealed record ObtenerListaComprasQuery(
    DateTime? FechaDesde,
    DateTime? FechaHasta,
    Guid?     ProveedorId,
    Guid?     UsuarioRegistroId,
    string?   FiltroEstado,        // "vigente" | "anulada" | null (todas)
    string?   NumeroComprobante
) : IRequest<List<ObtenerListaComprasResponse>>;
