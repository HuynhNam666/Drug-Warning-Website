using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MedicineWarningAPI.Models;
using Microsoft.IdentityModel.Tokens;

namespace MedicineWarningAPI.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Admin admin, out DateTime expiresAt)
        {
            var key = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Missing Jwt:Key");

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var expireMinutes = int.TryParse(
                _configuration["Jwt:ExpireMinutes"],
                out var minutes
            ) ? minutes : 120;

            expiresAt = DateTime.UtcNow.AddMinutes(expireMinutes);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                new Claim(ClaimTypes.Name, admin.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            if (!string.IsNullOrWhiteSpace(admin.FullName))
            {
                claims.Add(new Claim("FullName", admin.FullName));
            }

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key)
            );

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}