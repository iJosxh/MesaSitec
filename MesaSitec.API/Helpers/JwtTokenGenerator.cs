using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MesaSitec.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace MesaSitec.API.Helpers;

public class JwtTokenGenerator
{
    private readonly string _secret;

    public JwtTokenGenerator(string secret)
    {
        _secret = secret;
    }

    public string GenerateToken(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim("tenantId", usuario.TenantId.ToString()),
            new Claim("rol", usuario.Rol.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_secret));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}