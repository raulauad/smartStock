namespace smartStock.Api.Domain.Models;

public class CierreCaja
{
    public int      Id           { get; set; }
    public DateTime FechaCierre  { get; set; } = DateTime.UtcNow;
    public decimal  TotalVentas  { get; set; }
    public decimal  TotalCompras { get; set; }
    public decimal  SaldoFinal   { get; set; }   // calculado en app: Ventas - Compras

    public int?          SesionVentaId  { get; set; }   // unique → 1:1
    public SesionVenta?  SesionVenta    { get; set; }
    public int?          SesionCompraId { get; set; }   // unique → 1:1
    public SesionCompra? SesionCompra   { get; set; }
}
