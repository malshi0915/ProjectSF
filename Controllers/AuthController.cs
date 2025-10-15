using BusBookingApi.Data;
using BusBookingApi.DTOs;
using BusBookingApi.Models;
using BusBookingApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace BusBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IJwtService _jwt;
        public AuthController(AppDbContext db, IJwtService jwt) { _db = db; _jwt = jwt; }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Email == dto.Email)) return BadRequest("Email already in use.");
            var salt = RandomNumberGenerator.GetBytes(128 / 8);
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(dto.Password, salt, KeyDerivationPrf.HMACSHA256, 10000, 256 / 8));
            var user = new User { FullName = dto.FullName, Email = dto.Email, PasswordHash = $"{Convert.ToBase64String(salt)}:{hash}" };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            var token = _jwt.GenerateToken(user);
            return Ok(new AuthResponse { Token = token, Email = user.Email, FullName = user.FullName });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) return Unauthorized("Invalid credentials.");
            var parts = user.PasswordHash.Split(':');
            var salt = Convert.FromBase64String(parts[0]);
            var storedHash = parts[1];
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(dto.Password, salt, KeyDerivationPrf.HMACSHA256, 10000, 256 / 8));
            if (hash != storedHash) return Unauthorized("Invalid credentials.");
            var token = _jwt.GenerateToken(user);
            return Ok(new AuthResponse { Token = token, Email = user.Email, FullName = user.FullName });
        }
    }
}