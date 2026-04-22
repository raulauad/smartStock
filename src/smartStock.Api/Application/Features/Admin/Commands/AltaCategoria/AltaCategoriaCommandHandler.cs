using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Application.Common.Exceptions.Categorias;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Domain.Models;
using smartStock.Shared.Dtos.Admin.AltaCategoria;

namespace smartStock.Api.Application.Features.Admin.Commands.AltaCategoria;

public sealed class AltaCategoriaCommandHandler
    : IRequestHandler<AltaCategoriaCommand, AltaCategoriaResponse>
{
    private readonly ICategoriaRepository _categoriaRepository;

    public AltaCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
        => _categoriaRepository = categoriaRepository;

    public async Task<AltaCategoriaResponse> Handle(
        AltaCategoriaCommand command,
        CancellationToken    cancellationToken)
    {
        var nombre = command.Nombre.Trim();

        if (await _categoriaRepository.NombreExisteAsync(nombre, null, cancellationToken))
            throw new CategoriaNombreDuplicadoException();

        var categoria = new Categoria
        {
            Id            = Guid.NewGuid(),
            Nombre        = nombre,
            Descripcion   = command.Descripcion?.Trim(),
            EstaActivo    = true,
            FechaAlta     = DateTime.UtcNow,
            UsuarioAltaId = command.UsuarioAltaId
        };

        try
        {
            await _categoriaRepository.CrearAsync(categoria, cancellationToken);
        }
        catch (DbUpdateException)
        {
            if (await _categoriaRepository.NombreExisteAsync(nombre, null, cancellationToken))
                throw new CategoriaNombreDuplicadoException();
            throw;
        }

        return new AltaCategoriaResponse(categoria.Id, categoria.Nombre, categoria.Descripcion, categoria.FechaAlta);
    }
}
