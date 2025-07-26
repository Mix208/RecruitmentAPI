namespace RecruitmentAPI.API.Models
{
    public enum UserRole { Candidat, Recruteur }

    public class User
    {
        public int Id { get; set; } // Clé primaire
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.Candidat;

        // Relation : un recruteur peut publier plusieurs jobs
        public ICollection<Job>? JobsPosted { get; set; }

        // Relation : un candidat peut avoir plusieurs candidatures
        public ICollection<Application>? Applications { get; set; }
    }
}
