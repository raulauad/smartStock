using MediatR;
using smartStock.Api.Domain.Enums;
using smartStock.Shared.Dtos.Admin.EditarProducto;

namespace smartStock.Api.Application.Features.Admin.Commands.EditarProducto;

public sealed record EditarProductoCommand(
    string       Nombre,
    Guid         CategoriaId,
    UnidadMedida UnidadMedida,
    decimal      PrecioCosto,
    decimal      PrecioVenta,
    decimal      StockMinimo,
    bool         ConfirmarPrecioVentaMenorCosto,
    bool         ConfirmarNombreSimilar
) : IRequest<EditarProductoResponse>
{
    // Asignado por el controller desde la ruta — nunca del body.
    public Guid ProductoId { get; init; }
}
