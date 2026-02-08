using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using todofinal.Application.DTOs;
using todofinal.Domain.Entities;
using todofinal.Infrastructure.Persistence; // Kendi context yoluna göre düzenle

namespace todofinal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context; // Veritabanı bağlantısı
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    // 1. KAYIT OLMA (Register) - 404 hatasını bu metodun olmaması çözecek
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
    {
        // Kullanıcı adı daha önce alınmış mı kontrolü
        if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            return BadRequest("Bu kullanıcı adı zaten alınmış.");

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            // Şifreyi BCrypt ile hashliyoruz (Güvenlik!)
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password) 
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Kayıt başarıyla tamamlandı!" });
    }

    // 2. GİRİŞ YAPMA (Login)
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        // Veritabanından kullanıcıyı bul
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == login.Username);

        // Kullanıcı yoksa veya şifre yanlışsa (BCrypt.Verify ile kontrol edilir)
        if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı!" });
        }

        var token = CreateToken(user);
        return Ok(new { token });
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            // BURASI KRİTİK: user.Id veritabanından gelen gerçek ID olmalı
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}