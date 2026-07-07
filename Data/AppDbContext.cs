using Microsoft.EntityFrameworkCore;
using MyWebPage.Models;

namespace MyWebPage.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<PersonalInfo> PersonalInfos { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<ProjectImage> ProjectImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Seed some initial personal info data to prevent errors on first run
            modelBuilder.Entity<PersonalInfo>().HasData(
                new PersonalInfo 
                { 
                    Id = 1, 
                    FullName = "Adınız Soyadınız", 
                    Title = "Full Stack Developer", 
                    AboutMe = "Web geliştirme konusunda tutkuluyum.", 
                    ContactEmail = "email@example.com",
                    LinkedInUrl = "https://www.linkedin.com/in/muhammetbag/",
                    GitHubUrl = "https://github.com/muhammetbag"
                }
            );
        }
    }
}
