using MediatR;
using smartStock.Api.Application.Common.Exceptions.Categorias;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Shared.Dtos.Admin.CambiarEstadoCategoria;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Commands.CambiarEstadoCategoria;

public sealed class CambiarEstadoCategoriaCommandHandler
    : IRequestHandler<CambiarEstadoCategoriaCommand, CambiarEstadoCategoriaResponse>
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CambiarEstadoCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
        => _categoriaRepository = categoriaRepository;

    public async Task<CambiarEstadoCategoriaResponse> Handle(
        CambiarEstadoCategoriaCommand command,
        CancellationToken             cancellationToken)
    {
        var categoria = await _categoriaRepository.ObtenerPorIdAsync(command.CategoriaId, cancellationToken)
            ?? throw new CategoriaNoEncontradaException();

        if (categoria.EstaActivo == command.EstaActivo)
            throw new EstadoCategoriaSinCambioException(command.EstaActivo);

        var productosReasignados = 0;

        if (!command.EstaActivo)
        {
            var tieneProductos = await _categoriaRepository.TieneProductosAsync(command.CategoriaId, cancellationToken);

            if (tieneProductos)
            {
                if (!await _categoriaRepository.ExistenActivasAlternativasAsync(command.CategoriaId, cancellationToken))
                    throw new SinCategoriasActivasAlternativasException();

                if (command.CategoriaDestinoId is null)
                    throw new CategoriaReasignacionRequiereDestinoException();

                var destino = await _categoriaRepository.ObtenerPorIdAsync(command.CategoriaDestinoId.Value, cancellationToken);

                if (destino is null || !destino.EstaActivo || destino.Id == command.CategoriaId)
                    throw new CategoriaDestinoInvalidaException();

                productosReasignados = await _categoriaRepository.ReasignarProductosAsync(
                    command.CategoriaId, command.CategoriaDestinoId.Value, cancellationToken);
            }
        }

        categoria.EstaActivo = command.EstaActivo;
        await _categoriaRepository.ActualizarAsync(categoria, cancellationToken);

        return new CambiarEstadoCategoriaResponse(categoria.Nombre, categoria.EstaActivo, productosReasignados);
    }
}
