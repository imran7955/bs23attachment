using DbLoadingTest;
using Microsoft.EntityFrameworkCore;

ApplicationDbContext dbContext = new();
/* Run only once to seed the database    
dbContext.Courses.Add(new Course
{
    Title = "Math",
    Students = new List<Student>
    {
        new Student { Name = "Alice" },
        new Student { Name = "Bob" }
    }
});

dbContext.Courses.Add(new Course
{
    Title = "Science",
    Students = new List<Student>
    {
        new Student { Name = "Charlie" },
        new Student { Name = "David" }
    }
});

dbContext.SaveChanges();

//*/

var courses = dbContext.Courses
    //.Include(c => c.Students)
    .ToList();

Console.WriteLine(courses);
