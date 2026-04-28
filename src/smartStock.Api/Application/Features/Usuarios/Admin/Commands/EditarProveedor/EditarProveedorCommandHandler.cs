using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Application.Common.Exceptions.Proveedores;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Shared.Dtos.Admin.EditarProveedor;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Commands.EditarProveedor;

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

        var email = command.Email.ToLowerInvariant();

        // FA2: CUIT duplicado en otro proveedor
        if (command.Cuit is not null &&
            await _proveedorRepository.CuitExisteAsync(command.Cuit, command.ProveedorId, cancellationToken))
            throw new CuitDuplicadoException();

        // FA3: unicidad de nombre, email y teléfono en otro proveedor
        if (await _proveedorRepository.NombreExisteAsync(command.Nombre, command.ProveedorId, cancellationToken))
            throw new NombreDuplicadoException();

        if (await _proveedorRepository.EmailExisteAsync(email, command.ProveedorId, cancellationToken))
            throw new EmailProveedorDuplicadoException();

        if (await _proveedorRepository.TelefonoExisteAsync(command.Telefono, command.ProveedorId, cancellationToken))
            throw new TelefonoDuplicadoException();

        proveedor.Nombre              = command.Nombre;
        proveedor.Cuit                = command.Cuit;
        proveedor.Telefono            = command.Telefono;
        proveedor.Email               = email;
        proveedor.Direccion.Pais      = command.Direccion.Pais;
        proveedor.Direccion.Provincia = command.Direccion.Provincia;
        proveedor.Direccion.Localidad = command.Direccion.Localidad;
        proveedor.Direccion.CodigoPostal = command.Direccion.CodigoPostal;
        proveedor.Direccion.Calle     = command.Direccion.Calle;
        proveedor.Direccion.Numero    = command.Direccion.Numero;
        proveedor.Observaciones       = command.Observaciones;

        try
        {
            await _proveedorRepository.ActualizarAsync(proveedor, cancellationToken);
        }
        catch (DbUpdateException)
        {
            if (await _proveedorRepository.NombreExisteAsync(command.Nombre, command.ProveedorId, cancellationToken))
                throw new NombreDuplicadoException();
            if (await _proveedorRepository.EmailExisteAsync(email, command.ProveedorId, cancellationToken))
                throw new EmailProveedorDuplicadoException();
            if (await _proveedorRepository.TelefonoExisteAsync(command.Telefono, command.ProveedorId, cancellationToken))
                throw new TelefonoDuplicadoException();
            if (command.Cuit is not null &&
                await _proveedorRepository.CuitExisteAsync(command.Cuit, command.ProveedorId, cancellationToken))
                throw new CuitDuplicadoException();
            throw;
        }

        return new EditarProveedorResponse(proveedor.Id, proveedor.Nombre, proveedor.Email, proveedor.Cuit);
    }
}
