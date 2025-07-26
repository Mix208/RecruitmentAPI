namespace RecruitmentAPI.API.DTOs.Auth
{
    // Données reçues lors du login
    public class UserLoginDto
    {
        public string Email { get; set; } = string.Empty;      // Email utilisé pour se connecter
        public string Password { get; set; } = string.Empty;   // Mot de passe en clair (à vérifier)
    }
}
