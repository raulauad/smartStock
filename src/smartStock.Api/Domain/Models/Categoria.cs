namespace smartStock.Api.Domain.Models;

public class Categoria
{
    public Guid    Id          { get; set; }
    public string  Nombre      { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public bool     EstaActivo    { get; set; } = true;
    public DateTime FechaAlta     { get; set; } = DateTime.UtcNow;
    public Guid     UsuarioAltaId { get; set; }
    public Usuario  UsuarioAlta   { get; set; } = null!;

    public ICollection<Producto> Productos { get; set; } = [];
}
