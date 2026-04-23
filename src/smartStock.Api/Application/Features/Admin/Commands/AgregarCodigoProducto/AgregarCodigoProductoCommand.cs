using MediatR;
using smartStock.Api.Domain.Enums;
using smartStock.Shared.Dtos.Admin.AgregarCodigoProducto;

namespace smartStock.Api.Application.Features.Admin.Commands.AgregarCodigoProducto;

public sealed record AgregarCodigoProductoCommand(
    string     Codigo,
    TipoCodigo Tipo,
    decimal    Factor,
    string?    Descripcion
) : IRequest<AgregarCodigoProductoResponse>
{
    // Asignado por el controller desde la ruta — nunca del body.
    public Guid ProductoId { get; init; }
}
