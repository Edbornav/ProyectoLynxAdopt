using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProyectoAdoptBack.Models;

namespace ProyectoAdoptBack.Services
{
    public interface ITokenService
    {
        string GenerarToken(Usuario usuario);
    }
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _JwtSettings;
        public TokenService(IOptions<JwtSettings> jwtsettings)
        {
            _JwtSettings = jwtsettings.Value;
        }
        public string GenerarToken(Usuario usuario)
        {
            var claims = new []
            {
               new Claim (ClaimTypes.NameIdentifier, usuario.UsuarioID.ToString()),
               new Claim (ClaimTypes.Email, usuario.Correo),
               new Claim(ClaimTypes.Role, usuario.TipoUsuario)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_JwtSettings.SecretKey) );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer : _JwtSettings.Issuer,
                audience : _JwtSettings.Audience,
                claims : claims,
                expires : DateTime.UtcNow.AddMinutes(_JwtSettings.ExpirationInMinutes),
                signingCredentials : creds

            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}