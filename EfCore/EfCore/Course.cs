namespace EfCore
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Student> Students { get; set; }
    }
}
