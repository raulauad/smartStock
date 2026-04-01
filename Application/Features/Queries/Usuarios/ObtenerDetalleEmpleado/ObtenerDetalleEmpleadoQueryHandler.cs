using MediatR;
using Microsoft.AspNetCore.Identity;
using smartStock.Application.Common.Exceptions.Usuarios;
using smartStock.Domain.Models;

namespace smartStock.Application.Features.Queries.Usuarios.ObtenerDetalleEmpleado;

public sealed class ObtenerDetalleEmpleadoQueryHandler
    : IRequestHandler<ObtenerDetalleEmpleadoQuery, ObtenerDetalleEmpleadoResponse>
{
    private const string RolEmpleado = "Empleado";

    private readonly UserManager<Usuario> _userManager;

    public ObtenerDetalleEmpleadoQueryHandler(UserManager<Usuario> userManager)
        => _userManager = userManager;

    public async Task<ObtenerDetalleEmpleadoResponse> Handle(
        ObtenerDetalleEmpleadoQuery query,
        CancellationToken           cancellationToken)
    {
        var empleado = await _userManager.FindByIdAsync(query.EmpleadoId.ToString())
            ?? throw new UsuarioNoEncontradoException();

        var esEmpleado = await _userManager.IsInRoleAsync(empleado, RolEmpleado);
        if (!esEmpleado)
            throw new UsuarioNoEncontradoException();

        return new ObtenerDetalleEmpleadoResponse(
            empleado.Id,
            empleado.Nombre,
            empleado.Email!,
            empleado.Telefono,
            empleado.Dni,
            new DireccionEmpleadoResponse(
                empleado.Direccion.Pais,
                empleado.Direccion.Provincia,
                empleado.Direccion.Localidad,
                empleado.Direccion.CodigoPostal,
                empleado.Direccion.Calle,
                empleado.Direccion.Numero));
    }
}
