namespace RecruitmentAPI.API.Models
{
    public enum ApplicationStatus { EnAttente, Acceptee, Refusee }

    public class Application
    {
        public int Id { get; set; }
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        public ApplicationStatus Status { get; set; } = ApplicationStatus.EnAttente;

        // Relations : candidat + job
        public int UserId { get; set; }
        public User? User { get; set; }

        public int JobId { get; set; }
        public Job? Job { get; set; }
    }
}
