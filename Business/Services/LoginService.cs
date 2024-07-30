using ApiEventos.Data;
using ApiEventos.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiEventos.Services
{
    public class LoginService : ILoginService
    {
        private readonly DwiApieventosContext _context;
        private readonly IConfiguration _configuration;

        public LoginService(DwiApieventosContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponse?> Login(LoginRequest loginRequest)
        {
            var usuario = await GetUsuario(loginRequest);

            if (usuario == null) return null;

            return await Task.FromResult(GenerateToken(usuario));
        }

        private async Task<Usuario?> GetUsuario(LoginRequest loginRequest){
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.CorreoElectronico == loginRequest.CorreoElectronico && u.Contraseña == loginRequest.Contraseña);
            return usuario;
        }

        private LoginResponse GenerateToken(Usuario usuario){
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Role, usuario.EsAdmin ? "Admin" : "User")
            };
            
            var _key = _configuration.GetSection("Jwt:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key is not null? _key : ""));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var expiration = DateTime.Now.AddHours(1);
            
            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return new LoginResponse
            {
                Token = token,
                Expiration = expiration
            };
        }
    }
}
