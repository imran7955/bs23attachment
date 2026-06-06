using EfCore;
using Microsoft.EntityFrameworkCore;

using ApplicationDbContext dbContext = new();

// Ensure database exists and matches your migrations
dbContext.Database.EnsureCreated();

Console.WriteLine("=== EF Core Observer ===");
PrintDatabaseState(dbContext);

while (true)
{
    Console.WriteLine("\nChoose an operation strategy:");
    Console.WriteLine("1. Insert a Course");
    Console.WriteLine("2. Insert a Student");
    Console.WriteLine("3. Delete a Course Completely (Using ID)");
    Console.WriteLine("4. Delete a Student Completely (Using ID)");
    Console.WriteLine("5. Exit");
    Console.Write("Enter choice (1-5): ");

    string choice = Console.ReadLine();

    if (choice == "5") break;

    switch (choice)
    {
        case "1":
            AddCourse(dbContext);
            PrintDatabaseState(dbContext);
            break;
        case "2":
            AddStudent(dbContext);
            PrintDatabaseState(dbContext);
            break;
        case "3":
            DeleteCourseCompletely(dbContext);
            PrintDatabaseState(dbContext);
            break;
        case "4":
            DeleteStudentCompletely(dbContext);
            PrintDatabaseState(dbContext);
            break;
        default:
            Console.WriteLine("Invalid option. Try again.");
            break;
    }
}

static void AddCourse(ApplicationDbContext dbContext)
{
    Console.Write("\nEnter New Course Title: ");
    string courseTitle = Console.ReadLine();

    Console.Write("How many students do you want to add to this course? ");
    if (!int.TryParse(Console.ReadLine(), out int studentCount) || studentCount <= 0)
    {
        Console.WriteLine("Invalid number. Operation cancelled.");
        return;
    }

    var newCourse = new Course
    {
        Title = courseTitle,
        Students = new List<Student>()
    };

    for (int i = 1; i <= studentCount; i++)
    {
        Console.Write($"Enter name for Student {i}: ");
        string name = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(name))
        {
            newCourse.Students.Add(new Student { Name = name });
        }
    }

    Console.WriteLine("\n[EF CORE WORKLOG] Saving to database...");
    dbContext.Courses.Add(newCourse);
    dbContext.SaveChanges();
    Console.WriteLine("[EF CORE WORKLOG] Save complete.\n");
}

static void AddStudent(ApplicationDbContext dbContext)
{
    Console.Write("\nEnter New Student Name: ");
    string studentName = Console.ReadLine();

    Console.Write("How many course this student took? ");
    if (!int.TryParse(Console.ReadLine(), out int courseInputCount) || courseInputCount <= 0)
    {
        Console.WriteLine("Invalid number. Operation cancelled.");
        return;
    }

    var newStudent = new Student
    {
        Name = studentName,
        Courses = new List<Course>()
    };

    var processedTitles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    for (int i = 1; i <= courseInputCount; i++)
    {
        Console.Write($"Enter course title(s) for input line {i} (space-separated allowed): ");
        string rawInput = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(rawInput)) continue;

        string[] titlesInLine = rawInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (string title in titlesInLine)
        {
            if (processedTitles.Contains(title)) continue;
            processedTitles.Add(title);

            var existingCourse = dbContext.Courses
                .FirstOrDefault(c => c.Title.ToLower() == title.ToLower());

            if (existingCourse != null)
            {
                newStudent.Courses.Add(existingCourse);
                Console.WriteLine($" -> Found existing course '{existingCourse.Title}' (ID: {existingCourse.Id}). Will link to it.");
            }
            else
            {
                var newCourse = new Course { Title = title };
                newStudent.Courses.Add(newCourse);
                Console.WriteLine($" -> '{title}' not found in database. A new course row will be created.");
            }
        }
    }

    Console.WriteLine("\n[EF CORE WORKLOG] Executing SaveChanges(). Pay close attention to the SQL logs...");
    dbContext.Students.Add(newStudent);
    dbContext.SaveChanges();
    Console.WriteLine("[EF CORE WORKLOG] Save complete.\n");
}

// UPDATED FEATURE 3: Finds and drops a course row using its primary key ID
static void DeleteCourseCompletely(ApplicationDbContext dbContext)
{
    Console.Write("\nEnter the ID of the Course you want to DELETE: ");
    if (!int.TryParse(Console.ReadLine(), out int courseId))
    {
        Console.WriteLine("Invalid ID format. Must be an integer.");
        return;
    }

    // .Find() is an optimized EF Core method specifically designed to search by Primary Key ID
    var courseToDelete = dbContext.Courses.Find(courseId);

    if (courseToDelete == null)
    {
        Console.WriteLine($"Course with ID {courseId} not found.");
        return;
    }

    dbContext.Courses.Remove(courseToDelete);

    Console.WriteLine("\n[EF CORE WORKLOG] Executing SaveChanges(). Watch the ID clean up step in the SQL cascade...");
    dbContext.SaveChanges();
    Console.WriteLine($"[EF CORE WORKLOG] Course with ID {courseId} completely removed from database.\n");
}

// UPDATED FEATURE 4: Finds and drops a student row using its primary key ID
static void DeleteStudentCompletely(ApplicationDbContext dbContext)
{
    Console.Write("\nEnter the ID of the Student you want to DELETE: ");
    if (!int.TryParse(Console.ReadLine(), out int studentId))
    {
        Console.WriteLine("Invalid ID format. Must be an integer.");
        return;
    }

    // Using .Find() to directly pick the entity by its tracking index key
    var studentToDelete = dbContext.Students.Find(studentId);

    if (studentToDelete == null)
    {
        Console.WriteLine($"Student with ID {studentId} not found.");
        return;
    }

    dbContext.Students.Remove(studentToDelete);

    Console.WriteLine("\n[EF CORE WORKLOG] Executing SaveChanges(). Watch the ID clean up step in the SQL cascade...");
    dbContext.SaveChanges();
    Console.WriteLine($"[EF CORE WORKLOG] Student with ID {studentId} completely removed from database.\n");
}

static void PrintDatabaseState(ApplicationDbContext dbContext)
{
    Console.WriteLine("\n================ CURRENT DATABASE STATE ================");

    var courses = dbContext.Courses.Include(c => c.Students).AsNoTracking().ToList();

    Console.WriteLine("--- COURSES TABLE (with enrolled students) ---");
    if (!courses.Any()) Console.WriteLine("(No courses found)");
    foreach (var c in courses)
    {
        var studentNames = c.Students != null && c.Students.Any()
            ? string.Join(", ", c.Students.Select(s => $"{s.Name} (ID:{s.Id})"))
            : "None";

        Console.WriteLine($"[ID: {c.Id}] Title: {c.Title} | Enrolled Students: [{studentNames}]");
    }

    var students = dbContext.Students.Include(s => s.Courses).AsNoTracking().ToList();

    Console.WriteLine("\n--- STUDENTS TABLE (with assigned courses) ---");
    if (!students.Any()) Console.WriteLine("(No students found)");
    foreach (var s in students)
    {
        var courseTitles = s.Courses != null && s.Courses.Any()
            ? string.Join(", ", s.Courses.Select(c => $"{c.Title} (ID:{c.Id})"))
            : "None";

        Console.WriteLine($"[ID: {s.Id}] Name: {s.Name} | Assigned Courses: [{courseTitles}]");
    }

    Console.WriteLine("========================================================");
}



