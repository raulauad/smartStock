using MediatR;
using smartStock.Shared.Dtos.Ventas.AnularVenta;

namespace smartStock.Api.Application.Features.Ventas.Commands.AnularVenta;

public sealed record AnularVentaCommand : IRequest<AnularVentaResponse>
{
    public int    VentaId         { get; init; }   // inyectado desde ruta en controller
    public Guid   UsuarioId       { get; init; }   // inyectado desde JWT en controller
    public string MotivoAnulacion { get; init; } = string.Empty;
}
