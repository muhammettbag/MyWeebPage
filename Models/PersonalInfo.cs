using System.ComponentModel.DataAnnotations;

namespace MyWebPage.Models
{
    public class PersonalInfo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string AboutMe { get; set; }

        [EmailAddress]
        public string ContactEmail { get; set; }

        [Url]
        public string LinkedInUrl { get; set; }

        [Url]
        public string GitHubUrl { get; set; }

        public string? CvUrl { get; set; }
    }
}
