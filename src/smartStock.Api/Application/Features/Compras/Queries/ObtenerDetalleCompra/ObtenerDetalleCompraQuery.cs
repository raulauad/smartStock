using MediatR;
using smartStock.Shared.Dtos.Compras.ObtenerDetalleCompra;

namespace smartStock.Api.Application.Features.Compras.Queries.ObtenerDetalleCompra;

public sealed record ObtenerDetalleCompraQuery(int CompraId) : IRequest<ObtenerDetalleCompraResponse>;
