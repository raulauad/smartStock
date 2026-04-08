using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using smartStock.Api.Application.Common.Interfaces;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Infrastructure.Services;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
        => _configuration = configuration;

    public (string Token, DateTime Expiracion) GenerarToken(Usuario usuario, IList<string> roles)
    {
        var jwtConfig   = _configuration.GetSection("Jwt");
        var key         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiracion  = DateTime.UtcNow.AddMinutes(double.Parse(jwtConfig["ExpirationMinutes"]!));

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,   usuario.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, usuario.Email!),
            new("nombre",                      usuario.Nombre),
            new(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
        };

        claims.AddRange(roles.Select(rol => new Claim(ClaimTypes.Role, rol)));

        var token = new JwtSecurityToken(
            issuer:             jwtConfig["Issuer"],
            audience:           jwtConfig["Audience"],
            claims:             claims,
            expires:            expiracion,
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiracion);
    }
}
