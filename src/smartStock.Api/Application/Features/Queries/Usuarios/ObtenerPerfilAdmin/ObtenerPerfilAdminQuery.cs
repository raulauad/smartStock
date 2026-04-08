using MediatR;

namespace smartStock.Api.Application.Features.Queries.Usuarios.ObtenerPerfilAdmin;

public sealed record ObtenerPerfilAdminQuery(Guid AdminId)
    : IRequest<ObtenerPerfilAdminResponse>;
