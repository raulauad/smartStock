namespace smartStock.Api.Domain.Models;

public class CierreCaja
{
    public int      Id           { get; set; }
    public DateTime FechaCierre  { get; set; } = DateTime.UtcNow;
    public decimal  TotalVentas  { get; set; }
    public decimal  TotalCompras { get; set; }
    public decimal  SaldoFinal   { get; set; }   // calculado en app: Ventas - Compras

    public int?      VentaDiaId  { get; set; }   // unique → 1:1
    public VentaDia? VentaDia   { get; set; }
    public int?      CompraDiaId { get; set; }   // unique → 1:1
    public CompraDia? CompraDia  { get; set; }
}
