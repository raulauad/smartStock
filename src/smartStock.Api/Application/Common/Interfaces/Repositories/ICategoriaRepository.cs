using smartStock.Api.Domain.Models;

namespace smartStock.Api.Application.Common.Interfaces.Repositories;

public interface ICategoriaRepository
{
    Task<Categoria?> ObtenerPorIdAsync             (Guid id,                                          CancellationToken ct = default);
    Task<bool>       NombreExisteAsync             (string nombre,  Guid? excluirId,                  CancellationToken ct = default);
    Task<bool>       TieneProductosAsync           (Guid categoriaId,                                 CancellationToken ct = default);
    Task<bool>       ExistenActivasAlternativasAsync(Guid excluirId,                                  CancellationToken ct = default);
    Task<int>        ReasignarProductosAsync        (Guid desdeId,   Guid hastaId,                    CancellationToken ct = default);
    Task             CrearAsync                    (Categoria categoria,                               CancellationToken ct = default);
    Task             ActualizarAsync               (Categoria categoria,                               CancellationToken ct = default);
}
