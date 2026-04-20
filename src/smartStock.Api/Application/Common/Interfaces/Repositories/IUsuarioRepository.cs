using smartStock.Api.Domain.Enums;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Application.Common.Interfaces.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?>       ObtenerPorEmailAsync     (string email, CancellationToken ct = default);
    Task<Usuario?>       ObtenerPorIdAsync        (Guid id,      CancellationToken ct = default);
    Task<bool>           EmailExisteAsync         (string email, Guid? excluirId,   CancellationToken ct = default);
    Task<bool>           DniExisteAsync           (string dni,   Guid? excluirId,   CancellationToken ct = default);
    Task<bool>           ExisteAdministradorAsync (CancellationToken ct = default);
    Task<bool>           EstaActivoAsync          (Guid id, CancellationToken ct = default);
    Task                 CrearAsync               (Usuario usuario, CancellationToken ct = default);
    Task                 ActualizarAsync          (Usuario usuario, CancellationToken ct = default);
    Task                 EliminarAsync            (Usuario usuario, CancellationToken ct = default);
}
