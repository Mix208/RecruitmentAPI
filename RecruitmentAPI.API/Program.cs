using RecruitmentAPI.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ✅ 1. Charger les paramètres JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secret = jwtSettings["Key"];
if (string.IsNullOrEmpty(secret))
    throw new Exception("La clé secrète JWT est manquante dans appsettings.json."); // Sécurité

var key = Encoding.ASCII.GetBytes(secret);

// ✅ 2. Ajouter les services
builder.Services.AddControllers();              // Active les contrôleurs
builder.Services.AddEndpointsApiExplorer();     // Swagger
builder.Services.AddSwaggerGen();               // UI Swagger

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ 3. Authentification JWT (déplacée ici AVANT builder.Build())
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// ✅ 4. Construire l'app
var app = builder.Build();

// ✅ 5. Configurer le pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();                     // HTTP ➜ HTTPS
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();                       // 🔐 Auth
app.Run();
