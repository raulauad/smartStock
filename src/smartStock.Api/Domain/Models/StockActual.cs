namespace smartStock.Api.Domain.Models;

public class StockActual
{
    public int      ProductoId            { get; set; }   // PK + FK 1:1
    public decimal  Cantidad              { get; set; }
    public DateTime UltimaActualizacion   { get; set; }

    public Producto Producto { get; set; } = null!;
}
