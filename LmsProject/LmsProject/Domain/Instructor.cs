using System.ComponentModel.DataAnnotations;

namespace LmsProject.Models
{
    public class Instructor
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        // Many-to-many relationship with Course (EF Core handles this join automatically)
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}