using MediatR;
using smartStock.Api.Application.Common.Exceptions.Proveedores;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Models;
using smartStock.Shared.Dtos.Admin.AltaProveedor;

namespace smartStock.Api.Application.Features.Admin.Commands.AltaProveedor;

public sealed class AltaProveedorCommandHandler
    : IRequestHandler<AltaProveedorCommand, AltaProveedorResponse>
{
    private readonly IProveedorRepository _proveedorRepository;

    public AltaProveedorCommandHandler(IProveedorRepository proveedorRepository)
        => _proveedorRepository = proveedorRepository;

    public async Task<AltaProveedorResponse> Handle(
        AltaProveedorCommand command,
        CancellationToken    cancellationToken)
    {
        // FA2: CUIT duplicado
        if (command.Cuit is not null &&
            await _proveedorRepository.CuitExisteAsync(command.Cuit, null, cancellationToken))
            throw new CuitDuplicadoException();

        // FA3: unicidad de nombre, email y teléfono
        if (await _proveedorRepository.NombreExisteAsync(command.Nombre, null, cancellationToken))
            throw new NombreDuplicadoException();

        if (await _proveedorRepository.EmailExisteAsync(command.Email, null, cancellationToken))
            throw new EmailProveedorDuplicadoException();

        if (await _proveedorRepository.TelefonoExisteAsync(command.Telefono, null, cancellationToken))
            throw new TelefonoDuplicadoException();

        var proveedor = new Proveedor
        {
            Nombre        = command.Nombre,
            Cuit          = command.Cuit,
            Telefono      = command.Telefono,
            Email         = command.Email,
            Direccion     = new Direccion
            {
                Pais         = command.Direccion.Pais,
                Provincia    = command.Direccion.Provincia,
                Localidad    = command.Direccion.Localidad,
                CodigoPostal = command.Direccion.CodigoPostal,
                Calle        = command.Direccion.Calle,
                Numero       = command.Direccion.Numero
            },
            Observaciones = command.Observaciones,
            EstaActivo    = true,
            FechaAlta     = DateTime.UtcNow,
            UsuarioAltaId = command.UsuarioAltaId
        };

        await _proveedorRepository.CrearAsync(proveedor, cancellationToken);

        return new AltaProveedorResponse(proveedor.Id, proveedor.Nombre, proveedor.Email, proveedor.Cuit);
    }
}
