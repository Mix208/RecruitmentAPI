using Microsoft.AspNetCore.Mvc;                               // Fournit ControllerBase, ActionResult, etc.
using Microsoft.EntityFrameworkCore;                          // Pour FirstOrDefaultAsync, AnyAsync, etc.
using Microsoft.AspNetCore.Authorization;                     // Pour [AllowAnonymous]
using System.IdentityModel.Tokens.Jwt;                        // Pour créer/écrire le JWT
using System.Security.Claims;                                 // Pour ajouter des "claims" dans le token
using Microsoft.IdentityModel.Tokens;                         // Pour SecurityKey, SigningCredentials
using System.Text;                                            // Pour Encoding.UTF8
using RecruitmentAPI.API.Data;                                // Pour AppDbContext
using RecruitmentAPI.API.Models;                              // Pour User, UserRole
using RecruitmentAPI.API.DTOs.Auth;                           // Pour les DTOs créés
using BCrypt.Net;                                             // Pour hasher et vérifier le mot de passe

namespace RecruitmentAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // POST: /api/auth/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(UserRegisterDto dto)
        {
            // 1) Vérifier si l'email est déjà utilisé
            var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (exists)
                return BadRequest("Cet email est déjà utilisé.");

            // 2) Valider le rôle
            if (!Enum.TryParse<UserRole>(dto.Role, ignoreCase: true, out var role))
                return BadRequest("Rôle invalide. Utilisez 'Candidat' ou 'Recruteur'.");
            // 2.1) Vérifier la longueur du mot de passe
            if (dto.Password.Length < 8)
                return BadRequest("Le mot de passe doit contenir au moins 8 caractères.");

            // 3) Hasher le mot de passe
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // 4) Créer le nouvel utilisateur
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = passwordHash,
                Role = role
            };

            // 5) Enregistrer l'utilisateur
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Inscription réussie.");
        }

        // POST: /api/auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Login(UserLoginDto dto)
        {
            // 1) Récupérer l'utilisateur
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return Unauthorized("Email ou mot de passe invalide.");

            // 2) Vérifier le mot de passe
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!isPasswordValid)
                return Unauthorized("Email ou mot de passe invalide.");

            // 3) Générer le JWT
            var token = GenerateJwtToken(user, out DateTime expiresAt);

            // 4) Retourner le token
            return Ok(new AuthResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt
            });
        }

        // 🔐 Génère un JWT avec l'Id, Email et Rôle
        private string GenerateJwtToken(User user, out DateTime expiresAt)
        {
            var jwtSection = _config.GetSection("JwtSettings");
            var jwtKey = jwtSection["Key"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new InvalidOperationException("JWT key is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireMinutes = int.Parse(jwtSection["ExpireMinutes"] ?? "60");
            expiresAt = DateTime.UtcNow.AddMinutes(expireMinutes);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()), // Important pour [Authorize(Roles = "...")]
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
