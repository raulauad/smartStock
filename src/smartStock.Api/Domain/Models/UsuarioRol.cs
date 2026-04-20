using smartStock.Api.Domain.Enums;

namespace smartStock.Api.Domain.Models;

public class UsuarioRol
{
    public Guid    UsuarioId { get; set; }
    public Rol     Rol       { get; set; }

    public Usuario Usuario   { get; set; } = null!;
}
