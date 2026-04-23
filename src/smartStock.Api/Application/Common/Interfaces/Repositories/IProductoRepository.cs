using smartStock.Api.Domain.Models;

namespace smartStock.Api.Application.Common.Interfaces.Repositories;

public interface IProductoRepository
{
    Task<Producto?>       ObtenerPorIdAsync          (Guid productoId,                          CancellationToken ct = default);
    Task<bool>            CodigoExisteAsync           (string codigo,   Guid? excluirProductoId, CancellationToken ct = default);
    Task<List<string>>    ObtenerNombresSimilaresAsync(string nombre,   Guid? excluirId,         CancellationToken ct = default);
    Task<string>          GenerarCodigoInternoAsync   (                                          CancellationToken ct = default);
    Task<CodigoProducto?> ObtenerCodigoPorIdAsync     (Guid codigoId,   Guid productoId,         CancellationToken ct = default);
    Task<int>             ContarCodigosAsync          (Guid productoId,                          CancellationToken ct = default);
    Task                  CrearAsync                  (Producto producto,                        CancellationToken ct = default);
    Task                  ActualizarAsync             (Producto producto,                        CancellationToken ct = default);
    Task                  AgregarCodigoAsync          (CodigoProducto codigo,                    CancellationToken ct = default);
    Task                  ActualizarCodigoAsync       (CodigoProducto codigo,                    CancellationToken ct = default);
    Task                  EliminarCodigoAsync         (CodigoProducto codigo,                    CancellationToken ct = default);
}
