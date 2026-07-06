using System.Collections.Generic;

namespace MyWebPage.Models
{
    public class HomeViewModel
    {
        public PersonalInfo PersonalInfo { get; set; }
        public List<Project> Projects { get; set; }
        public List<Experience> Experiences { get; set; } = new List<Experience>();
    }
}
