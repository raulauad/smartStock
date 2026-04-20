namespace smartStock.Api.Domain.Models;

public class TokenRevocado
{
    public int      Id         { get; set; }
    public string   Jti        { get; set; } = string.Empty;
    public DateTime Expiracion { get; set; }
    public Guid     UsuarioId  { get; set; }
}
