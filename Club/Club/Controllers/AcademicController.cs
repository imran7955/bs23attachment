using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Club.Data;
using Club.Domain;
using System.Diagnostics;
using System.Linq;

namespace Club.Controllers
{
    public class AcademicController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AcademicController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================================================================
        // UNIVERSAL ENTRY ROUTE: /Academic/Dashboard
        // =========================================================================
        public IActionResult Dashboard()
        {
            // Returns the single-page application framework without running partial pipelines instantly
            return View();
        }

        // =========================================================================
        // PIPELINE 1: Eager Loading (Dashboard View)
        // =========================================================================
        public IActionResult EagerDashboard()
        {
            // We use .Include() to instruct EF Core to write an explicit SQL JOIN.
            // This pulls all students and all their corresponding courses in exactly 1 database hit.
            var studentDashboardData = _context.Students
                .Include(s => s.Courses)
                .ToList();

            ViewBag.Strategy = "Eager Loading (Single SQL JOIN Command)";
            return View("Dashboard", studentDashboardData);
        }

        // =========================================================================
        // PIPELINE 2: Lazy Loading (Deferred Processing View)
        // =========================================================================
        public IActionResult LazyDashboard()
        {
            // Notice there is NO .Include() here. 
            // Query 1: EF Core initially queries ONLY the basic Student table.
            var students = _context.Students.ToList();

            // When the Razor view starts looping through each student and accesses '.Courses',
            // EF Core will fire an independent hidden SQL query for EVERY single student row.
            ViewBag.Strategy = "Lazy Loading (Deferred Proxy Queries)";
            return View("Dashboard", students);
        }

        // GET: /Academic/SeedData
        public IActionResult SeedData()
        {
            // Check if data already exists to avoid duplication
            if (!_context.Students.Any())
            {
                // 1. Create Meaningful Courses
                var cse1 = new Course { Name = "Graph Theory & Optimization", Credit = 3.0 };
                var cse2 = new Course { Name = "Machine Learning Systems", Credit = 4.0 };
                var cse3 = new Course { Name = "Database Design & Management", Credit = 3.0 };

                // 2. Create Students and Assign Courses (Many-to-Many Layout)
                var student1 = new Student { Name = "Imran Hassan" };
                student1.Courses.Add(cse1);
                student1.Courses.Add(cse2);

                var student2 = new Student { Name = "Sabbir Ahmed" };
                student2.Courses.Add(cse2);
                student2.Courses.Add(cse3);

                var student3 = new Student { Name = "Nadia Islam" };
                student3.Courses.Add(cse1);
                student3.Courses.Add(cse3);

                // 3. Save to SQL Server Database
                _context.Students.AddRange(student1, student2, student3);
                _context.SaveChanges();

                return Content("Database successfully seeded with 3 Academic Students and 3 University Courses! Navigate back to the dashboards to test.");
            }

            return Content("Database already contains records. No seeding required.");
        }

        // GET: /Academic/FetchEagerData
        public IActionResult FetchEagerData()
        {
            // Eager loading: Fetches students and courses in 1 single database roundtrip
            var data = _context.Students
                               .Include(s => s.Courses)
                               .ToList();

            ViewBag.StrategyUsed = "Eager Loading Optimized (1 SQL JOIN statement execution)";

            // Renders just the raw HTML table segment
            return PartialView("_DashboardTable", data);
        }

        // GET: /Academic/FetchLazyData
        public IActionResult FetchLazyData()
        {
            // Lazy loading: initially requests ONLY basic student details
            var data = _context.Students.ToList();

            ViewBag.StrategyUsed = "Lazy Loading Proxies (N+1 database execution loops triggered)";

            // Renders just the raw HTML table segment
            return PartialView("_DashboardTable", data);
        }
    }
}