namespace RecruitmentAPI.API.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ContractType { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Clé étrangère : recruteur qui a posté
        public int UserId { get; set; }
        public User? User { get; set; }

        public ICollection<Application>? Applications { get; set; }
    }
}
