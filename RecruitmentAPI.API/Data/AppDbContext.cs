using Microsoft.EntityFrameworkCore;
using RecruitmentAPI.API.Models;

namespace RecruitmentAPI.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        // Tables
        public DbSet<User> Users => Set<User>();
        public DbSet<Job> Jobs => Set<Job>();
        public DbSet<Application> Applications => Set<Application>();
    }
}
