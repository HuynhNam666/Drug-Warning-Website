using MedicineWarningAPI.Data;
using MedicineWarningAPI.DTOs;
using MedicineWarningAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicineWarningAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;

        public AuthController(
            AppDbContext context,
            IJwtService jwtService,
            IPasswordService passwordService)
        {
            _context = context;
            _jwtService = jwtService;
            _passwordService = passwordService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Vui lòng nhập username và password.");
            }

            var admin = await _context.Admins
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Username == request.Username.Trim());

            if (admin == null)
            {
                return Unauthorized("Sai tài khoản hoặc mật khẩu.");
            }

            var isValidPassword = _passwordService.VerifyPassword(
                request.Password,
                admin.PasswordHash
            );

            if (!isValidPassword)
            {
                return Unauthorized("Sai tài khoản hoặc mật khẩu.");
            }

            var token = _jwtService.GenerateToken(admin, out var expiresAt);

            return Ok(new LoginResponseDto
            {
                Token = token,
                Username = admin.Username,
                FullName = admin.FullName,
                ExpiresAt = expiresAt
            });
        }
    }
}
