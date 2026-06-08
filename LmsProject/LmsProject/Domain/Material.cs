using System.ComponentModel.DataAnnotations;

namespace LmsProject.Models
{
    public class Material
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Url]
        public string YouTubeLink { get; set; } = string.Empty;

        // Link to the explicit join table to handle many courses and sequence positioning
        public ICollection<CourseMaterial> CourseMaterials { get; set; } = new List<CourseMaterial>();
    }
}