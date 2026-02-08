using todofinal.Application.Services;
using Microsoft.EntityFrameworkCore;
using todofinal.Infrastructure.Persistence;
using todofinal.Application.Interfaces;
using FluentValidation;
using todofinal.Application.Validators;
using AutoMapper; // 1. Bu using'i ekle
using todofinal.Application.Mappings;
using todofinal.Middleware; // 2. MappingProfile'ı tanıması için ekle
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// 1. SERVİSLER (Malzemeler) - builder.Build()'den ÖNCE yazılır
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// PostgreSQL Bağlantısı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Repository Kaydı
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ITodoService, TodoService>();
// --- BURAYA EKLE ---
// AutoMapper Kaydı: Uygulama ayağa kalkarken MappingProfile sınıfını bulur
// ve içindeki kuralları (Entity -> DTO gibi) hafızaya alır.
builder.Services.AddAutoMapper(typeof(MappingProfile)); 
// ------------------
builder.Services.AddValidatorsFromAssemblyContaining<CreateTodoDtoValidator>();
// 2. UYGULAMA İNŞASI (Ocak açıldı)
var app = builder.Build();

// 3. ARA YAZILIMLAR (Yemek pişiyor)
app.UseMiddleware<ExceptionMiddleware>(); // Kendi yazdığımız middleware

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// --- BU İKİ SATIR KRİTİK ---
app.UseAuthentication(); // Önce kimlik kontrolü (Sen kimsin?)
app.UseAuthorization();  // Sonra yetki kontrolü (Bunu yapmaya iznin var mı?)
// ---------------------------
app.MapControllers(); 

app.Run();