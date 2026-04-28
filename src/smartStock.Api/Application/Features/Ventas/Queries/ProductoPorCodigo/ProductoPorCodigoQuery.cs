using MediatR;
using smartStock.Shared.Dtos.Ventas.ProductoPorCodigo;

namespace smartStock.Api.Application.Features.Ventas.Queries.ProductoPorCodigo;

public sealed record ProductoPorCodigoQuery(string Codigo, bool EsAdmin)
    : IRequest<ProductoPorCodigoResponse?>;
