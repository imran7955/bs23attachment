using System.ComponentModel.DataAnnotations;

namespace LmsProject.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Many-to-many relationship with Instructor
        public List<Instructor> Instructors { get; set; } = new List<Instructor>();
        // Explicit relationship to Materials via the join table to preserve sequence positioning
        public List<CourseMaterial> CourseMaterials { get; set; } = new List<CourseMaterial>();
    }
}


