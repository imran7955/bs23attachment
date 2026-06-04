namespace Club.Domain
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Initializing the list is a production best practice
        public virtual IList<Course> Courses { get; set; } = new List<Course>();
    }
}