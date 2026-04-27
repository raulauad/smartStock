namespace smartStock.Shared.Dtos.Compras.AnularCompra;

public sealed record AnularCompraResponse(
    int      Id,
    bool     EstaAnulada,
    DateTime FechaAnulacion,
    string   MotivoAnulacion,
    string   UsuarioAnulaNombre
);
