namespace Club.Domain
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Credit { get; set; }

        // Add this back-reference to complete the many-to-many relationship
        public virtual IList<Student> Students { get; set; } = new List<Student>();
    }
}