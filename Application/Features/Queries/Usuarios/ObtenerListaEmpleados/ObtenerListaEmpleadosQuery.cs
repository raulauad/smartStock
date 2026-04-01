using MediatR;

namespace smartStock.Application.Features.Queries.Usuarios.ObtenerListaEmpleados;

public sealed record ObtenerListaEmpleadosQuery
    : IRequest<IReadOnlyList<ObtenerListaEmpleadosResponse>>;
