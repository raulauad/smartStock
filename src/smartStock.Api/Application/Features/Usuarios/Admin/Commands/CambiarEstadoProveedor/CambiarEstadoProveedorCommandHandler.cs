using MediatR;
using smartStock.Api.Application.Common.Exceptions.Proveedores;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Shared.Dtos.Admin.CambiarEstadoProveedor;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Commands.CambiarEstadoProveedor;

public sealed class CambiarEstadoProveedorCommandHandler
    : IRequestHandler<CambiarEstadoProveedorCommand, CambiarEstadoProveedorResponse>
{
    private readonly IProveedorRepository _proveedorRepository;

    public CambiarEstadoProveedorCommandHandler(IProveedorRepository proveedorRepository)
        => _proveedorRepository = proveedorRepository;

    public async Task<CambiarEstadoProveedorResponse> Handle(
        CambiarEstadoProveedorCommand command,
        CancellationToken             cancellationToken)
    {
        var proveedor = await _proveedorRepository.ObtenerPorIdAsync(command.ProveedorId, cancellationToken)
            ?? throw new ProveedorNoEncontradoException();

        if (proveedor.EstaActivo == command.EstaActivo)
            throw new EstadoProveedorSinCambioException(command.EstaActivo);

        proveedor.EstaActivo = command.EstaActivo;

        await _proveedorRepository.ActualizarAsync(proveedor, cancellationToken);

        return new CambiarEstadoProveedorResponse(proveedor.Nombre, proveedor.EstaActivo);
    }
}
