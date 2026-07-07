using System.ComponentModel.DataAnnotations;

namespace MyWebPage.Models
{
    public class ProjectImage
    {
        public int Id { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int ProjectId { get; set; }
        
        public Project Project { get; set; }
    }
}
