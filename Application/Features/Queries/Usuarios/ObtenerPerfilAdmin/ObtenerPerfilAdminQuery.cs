using MediatR;

namespace smartStock.Application.Features.Queries.Usuarios.ObtenerPerfilAdmin;

public sealed record ObtenerPerfilAdminQuery(Guid AdminId)
    : IRequest<ObtenerPerfilAdminResponse>;
