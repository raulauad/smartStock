using MediatR;
using smartStock.Api.Domain.Enums;
using smartStock.Shared.Dtos.Admin.AltaProducto;

namespace smartStock.Api.Application.Features.Admin.Commands.AltaProducto;

public sealed record AltaProductoCommand(
    string              Nombre,
    Guid                CategoriaId,
    UnidadMedida        UnidadMedida,
    decimal             PrecioCosto,
    decimal             PrecioVenta,
    decimal             StockInicial,
    decimal             StockMinimo,
    List<CodigoInput>?  Codigos,
    bool                ConfirmarPrecioVentaMenorCosto,
    bool                ConfirmarNombreSimilar
) : IRequest<AltaProductoResponse>
{
    // Asignado por el controller desde el claim sub del JWT — nunca del body.
    public Guid UsuarioAltaId { get; init; }
}

public sealed record CodigoInput(
    string     Codigo,
    TipoCodigo Tipo,
    decimal    Factor,
    string?    Descripcion
);
