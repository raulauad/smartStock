using smartStock.Api.Domain.Models;

namespace smartStock.Api.Application.Common.Interfaces.Repositories;

public interface IProveedorRepository
{
    Task<Proveedor?> ObtenerPorIdAsync        (Guid id,                                        CancellationToken ct = default);
    Task<bool>       CuitExisteAsync          (string cuit,   Guid? excluirId,                 CancellationToken ct = default);
    Task<bool>       NombreExisteAsync  (string nombre,   Guid? excluirId, CancellationToken ct = default);
    Task<bool>       EmailExisteAsync   (string email,    Guid? excluirId, CancellationToken ct = default);
    Task<bool>       TelefonoExisteAsync(string telefono, Guid? excluirId, CancellationToken ct = default);
    Task             CrearAsync               (Proveedor proveedor,                             CancellationToken ct = default);
    Task             ActualizarAsync          (Proveedor proveedor,                             CancellationToken ct = default);
}
