using MediatR;
using smartStock.Shared.Dtos.Admin.ObtenerPerfilAdmin;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Queries.ObtenerPerfilAdmin;

public sealed record ObtenerPerfilAdminQuery(Guid AdminId)
    : IRequest<ObtenerPerfilAdminResponse>;
