using smartStock.Api.Domain.Models;

namespace smartStock.Api.Application.Common.Interfaces.Repositories;

public interface IProveedorRepository
{
    Task<Proveedor?> ObtenerPorIdAsync        (Guid id,                                        CancellationToken ct = default);
    Task<bool>       CuitExisteAsync          (string cuit,   Guid? excluirId,                 CancellationToken ct = default);
    Task<bool>       NombreEmailExisteAsync   (string nombre, string email,   Guid? excluirId, CancellationToken ct = default);
    Task<bool>       NombreTelefonoExisteAsync(string nombre, string telefono, Guid? excluirId, CancellationToken ct = default);
    Task             CrearAsync               (Proveedor proveedor,                             CancellationToken ct = default);
    Task             ActualizarAsync          (Proveedor proveedor,                             CancellationToken ct = default);
}
