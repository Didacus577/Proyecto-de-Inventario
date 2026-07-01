using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Inventario.DTOS;
using Microsoft.Extensions.Configuration;

namespace Inventario.BLL.Servicios
{
    public class AuthService
    {
        private readonly IConfiguration _config;

        public AuthService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerarToken(UsuarioDTO usuario)
        {
          
            var jwtSection = _config.GetSection("Jwt");
            var secretKey = jwtSection["Key"];
            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];

            if (string.IsNullOrEmpty(secretKey))
                throw new Exception("La clave secreta de JWT no está configurada.");

            var key = Encoding.ASCII.GetBytes(secretKey);

           
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()));
            claims.AddClaim(new Claim(ClaimTypes.Email, usuario.Correo ?? ""));
            claims.AddClaim(new Claim(ClaimTypes.Name, usuario.NombreUsuario ?? ""));

          
            if (!string.IsNullOrEmpty(usuario.NombreRol))
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, usuario.NombreRol));
            }
            else
            {               
                claims.AddClaim(new Claim(ClaimTypes.Role, usuario.IdRol.ToString()));
            }
            
            claims.AddClaim(new Claim("IdRol", usuario.IdRol.ToString()));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(1), 
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = issuer,
                Audience = audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(tokenConfig);
        }
    }
}