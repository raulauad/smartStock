using MediatR;
using smartStock.Shared.Dtos.Admin.ObtenerListaEmpleados;

namespace smartStock.Api.Application.Features.Admin.Queries.ObtenerListaEmpleados;

public sealed record ObtenerListaEmpleadosQuery
    : IRequest<IReadOnlyList<ObtenerListaEmpleadosResponse>>;
