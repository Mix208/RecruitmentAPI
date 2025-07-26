namespace RecruitmentAPI.API.DTOs.Auth
{
    // Réponse envoyée après un login réussi
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;      // Le JWT à utiliser dans les requêtes
        public DateTime ExpiresAt { get; set; }                // Date d'expiration du token
    }
}
