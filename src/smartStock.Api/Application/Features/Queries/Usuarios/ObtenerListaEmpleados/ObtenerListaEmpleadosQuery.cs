using MediatR;

namespace smartStock.Api.Application.Features.Queries.Usuarios.ObtenerListaEmpleados;

public sealed record ObtenerListaEmpleadosQuery
    : IRequest<IReadOnlyList<ObtenerListaEmpleadosResponse>>;
