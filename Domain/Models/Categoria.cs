namespace smartStock.Domain.Models;

public class Categoria
{
    public int    Id          { get; set; }
    public string Nombre      { get; set; } = string.Empty;  // max 50
    public string Descripcion { get; set; } = string.Empty;  // max 300

    public ICollection<Producto> Productos { get; set; } = [];
}
