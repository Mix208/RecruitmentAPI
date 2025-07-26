namespace RecruitmentAPI.API.DTOs.Auth
{
    // Données reçues lors de l'inscription
    public class UserRegisterDto
    {
        public string Username { get; set; } = string.Empty;   // Nom d'utilisateur
        public string Email { get; set; } = string.Empty;      // Email unique
        public string Password { get; set; } = string.Empty;   // Mot de passe en clair (sera hashé)
        public string Role { get; set; } = "Candidat";         // "Candidat" ou "Recruteur"
    }
}
