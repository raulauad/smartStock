using MediatR;
using smartStock.Api.Application.Common.Exceptions.Proveedores;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Models;
using smartStock.Shared.Dtos.Admin.EditarProveedor;

namespace smartStock.Api.Application.Features.Admin.Commands.EditarProveedor;

public sealed class EditarProveedorCommandHandler
    : IRequestHandler<EditarProveedorCommand, EditarProveedorResponse>
{
    private readonly IProveedorRepository _proveedorRepository;

    public EditarProveedorCommandHandler(IProveedorRepository proveedorRepository)
        => _proveedorRepository = proveedorRepository;

    public async Task<EditarProveedorResponse> Handle(
        EditarProveedorCommand command,
        CancellationToken      cancellationToken)
    {
        var proveedor = await _proveedorRepository.ObtenerPorIdAsync(command.ProveedorId, cancellationToken)
            ?? throw new ProveedorNoEncontradoException();

        // FA2: CUIT duplicado en otro proveedor
        if (command.Cuit is not null &&
            await _proveedorRepository.CuitExisteAsync(command.Cuit, command.ProveedorId, cancellationToken))
            throw new CuitDuplicadoException();

        // FA3: Nombre+Email duplicado en otro proveedor
        if (await _proveedorRepository.NombreEmailExisteAsync(command.Nombre, command.Email, command.ProveedorId, cancellationToken))
            throw new NombreEmailDuplicadoException();

        // FA3: Nombre+Teléfono duplicado en otro proveedor
        if (await _proveedorRepository.NombreTelefonoExisteAsync(command.Nombre, command.Telefono, command.ProveedorId, cancellationToken))
            throw new NombreTelefonoDuplicadoException();

        proveedor.Nombre        = command.Nombre;
        proveedor.Cuit          = command.Cuit;
        proveedor.Telefono      = command.Telefono;
        proveedor.Email         = command.Email;
        proveedor.Direccion     = new Direccion
        {
            Pais         = command.Direccion.Pais,
            Provincia    = command.Direccion.Provincia,
            Localidad    = command.Direccion.Localidad,
            CodigoPostal = command.Direccion.CodigoPostal,
            Calle        = command.Direccion.Calle,
            Numero       = command.Direccion.Numero
        };
        proveedor.Observaciones = command.Observaciones;

        await _proveedorRepository.ActualizarAsync(proveedor, cancellationToken);

        return new EditarProveedorResponse(proveedor.Id, proveedor.Nombre, proveedor.Email, proveedor.Cuit);
    }
}
