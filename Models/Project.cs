using System.ComponentModel.DataAnnotations;

namespace MyWebPage.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Summary { get; set; }

        [Required]
        public string Technologies { get; set; }

        public string? LiveLink { get; set; }
        
        public string? ImageUrl { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public Microsoft.AspNetCore.Http.IFormFile? ImageFile { get; set; }
    }
}
