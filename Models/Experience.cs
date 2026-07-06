using System.ComponentModel.DataAnnotations;

namespace MyWebPage.Models
{
    public class Experience
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pozisyon alanı zorunludur.")]
        [StringLength(100)]
        public string Position { get; set; }

        [Required(ErrorMessage = "Şirket adı zorunludur.")]
        [StringLength(100)]
        public string Company { get; set; }

        [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
        [StringLength(50)]
        public string StartDate { get; set; }

        [StringLength(50)]
        public string? EndDate { get; set; }

        public bool IsCurrent { get; set; }

        [Required(ErrorMessage = "Açıklama alanı zorunludur.")]
        public string Description { get; set; }
    }
}
