using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LmsProject.Data;
using LmsProject;
using LmsProject.Models;

namespace LmsProject.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Course/ViewCourses
        public async Task<IActionResult> ViewCourses(string domain, string instructor, string search)
        {
            // 1. Start a base query tracking related Instructors, materials link, and underlying Material details
            var courseQuery = _context.Courses
                .Include(c => c.Instructors)
                .Include(c => c.CourseMaterials)
                    .ThenInclude(cm => cm.Material)
                .AsQueryable();

            // 2. Filter by Domain Selection if a specific target is set
            if (!string.IsNullOrEmpty(domain) && domain != "All")
            {
                courseQuery = courseQuery.Where(c => c.Domain == domain);
            }

            // 3. Filter by explicit Instructor Dropdown Selection
            if (!string.IsNullOrEmpty(instructor) && instructor != "All")
            {
                courseQuery = courseQuery.Where(c => c.Instructors.Any(i => i.Name == instructor));
            }

            // 4. Filter by Instructor Text Search bar string matching
            if (!string.IsNullOrEmpty(search))
            {
                courseQuery = courseQuery.Where(c => c.Instructors.Any(i => i.Name.Contains(search)));
            }

            // 5. Package results cleanly into the ViewModel container
            var viewModel = new CourseViewModel
            {
                Courses = await courseQuery.ToListAsync(),

                // Pull distinct, existing domains dynamically from the course table for the filter list
                Domains = await _context.Courses.Select(c => c.Domain).Distinct().ToListAsync(),
                Instructors = await _context.Instructors.ToListAsync(),

                // Preserve the state selections so the dropdown elements stay populated accurately
                SelectedDomain = domain ?? "All",
                SelectedInstructor = instructor ?? "All",
                SearchTerm = search ?? string.Empty
            };

            return View(viewModel);
        }

        // GET: Course/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Instructors)
                .Include(c => c.CourseMaterials)
                    .ThenInclude(cm => cm.Material)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            // Explicitly renders your custom-named view file while passing down the data model
            return View(course);
        }
    }
}