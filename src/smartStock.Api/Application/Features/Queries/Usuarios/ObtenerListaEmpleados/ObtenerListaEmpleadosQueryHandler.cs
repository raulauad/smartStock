using MediatR;
using Microsoft.AspNetCore.Identity;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Application.Features.Queries.Usuarios.ObtenerListaEmpleados;

public sealed class ObtenerListaEmpleadosQueryHandler
    : IRequestHandler<ObtenerListaEmpleadosQuery, IReadOnlyList<ObtenerListaEmpleadosResponse>>
{
    private const string RolEmpleado = "Empleado";

    private readonly UserManager<Usuario> _userManager;

    public ObtenerListaEmpleadosQueryHandler(UserManager<Usuario> userManager)
        => _userManager = userManager;

    public async Task<IReadOnlyList<ObtenerListaEmpleadosResponse>> Handle(
        ObtenerListaEmpleadosQuery query,
        CancellationToken          cancellationToken)
    {
        var empleados = await _userManager.GetUsersInRoleAsync(RolEmpleado);

        return empleados
            .Select(e => new ObtenerListaEmpleadosResponse(
                e.Id,
                e.Nombre,
                e.Email!,
                e.EstaActivo))
            .ToList();
    }
}
