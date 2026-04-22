using MediatR;
using Microsoft.EntityFrameworkCore;
using smartStock.Api.Application.Common.Exceptions.Categorias;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Shared.Dtos.Admin.EditarCategoria;

namespace smartStock.Api.Application.Features.Admin.Commands.EditarCategoria;

public sealed class EditarCategoriaCommandHandler
    : IRequestHandler<EditarCategoriaCommand, EditarCategoriaResponse>
{
    private readonly ICategoriaRepository _categoriaRepository;

    public EditarCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
        => _categoriaRepository = categoriaRepository;

    public async Task<EditarCategoriaResponse> Handle(
        EditarCategoriaCommand command,
        CancellationToken      cancellationToken)
    {
        var categoria = await _categoriaRepository.ObtenerPorIdAsync(command.CategoriaId, cancellationToken)
            ?? throw new CategoriaNoEncontradaException();

        var nombre = command.Nombre.Trim();

        if (await _categoriaRepository.NombreExisteAsync(nombre, command.CategoriaId, cancellationToken))
            throw new CategoriaNombreDuplicadoException();

        categoria.Nombre      = nombre;
        categoria.Descripcion = command.Descripcion?.Trim();

        try
        {
            await _categoriaRepository.ActualizarAsync(categoria, cancellationToken);
        }
        catch (DbUpdateException)
        {
            if (await _categoriaRepository.NombreExisteAsync(nombre, command.CategoriaId, cancellationToken))
                throw new CategoriaNombreDuplicadoException();
            throw;
        }

        return new EditarCategoriaResponse(categoria.Id, categoria.Nombre, categoria.Descripcion);
    }
}
