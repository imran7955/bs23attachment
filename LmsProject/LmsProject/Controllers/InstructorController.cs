using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LmsProject.Data;
using LmsProject.Models;

namespace LmsProject.Controllers
{
    public class InstructorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InstructorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: Instructor/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var courses = await _context.Courses
                .Include(c => c.Instructors)
                .Include(c => c.CourseMaterials)
                    .ThenInclude(cm => cm.Material)
                .ToListAsync();

            var instructors = await _context.Instructors.ToListAsync();

            ViewBag.Instructors = instructors;
            return View(courses);
        }

        // 2. POST: Instructor/CreateInstructor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInstructor(Instructor instructor)
        {
            if (!string.IsNullOrEmpty(instructor.Name))
            {
                _context.Instructors.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Dashboard));
            }
            return RedirectToAction(nameof(Dashboard));
        }

        // 3. GET: Instructor/CreateCourse
        public async Task<IActionResult> CreateCourse()
        {
            ViewBag.Instructors = await _context.Instructors.ToListAsync();

            // Hardcoded dynamic list of standard structural development domains for your dropdown
            ViewBag.Domains = new List<string>
            {
                ".NET Development",
                "Machine Learning",
                "Graph Theory",
                "Web Development",
                "Competitive Programming"
            };

            return View();
        }

        // 4. POST: Instructor/CreateCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(Course course, int[] selectedInstructors)
        {
            if (ModelState.IsValid)
            {
                // Link selected instructors from checkboxes to this course
                if (selectedInstructors != null)
                {
                    foreach (var id in selectedInstructors)
                    {
                        var instructor = await _context.Instructors.FindAsync(id);
                        if (instructor != null)
                        {
                            course.Instructors.Add(instructor);
                        }
                    }
                }

                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Dashboard));
            }

            ViewBag.Instructors = await _context.Instructors.ToListAsync();
            return View(course);
        }

        // 5. GET: Instructor/ManageSyllabus/{id}
        public async Task<IActionResult> ManageSyllabus(int id)
        {
            var course = await _context.Courses
                .Include(c => c.CourseMaterials)
                    .ThenInclude(cm => cm.Material)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) return NotFound();

            return View(course);
        }

        // 6. POST: Instructor/AddMaterialToCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMaterialToCourse(int courseId, string title, string url, int position)
        {
            var course = await _context.Courses
                .Include(c => c.CourseMaterials)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null) return NotFound();

            // Create new backing material instance
            var material = new Material { Title = title, YouTubeLink = url };
            _context.Materials.Add(material);
            await _context.SaveChangesAsync();

            // Create explicit tracking mapping row
            var courseMaterial = new CourseMaterial
            {
                CourseId = courseId,
                MaterialId = material.Id,
                Position = position
            };

            _context.CourseMaterials.Add(courseMaterial);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageSyllabus), new { id = courseId });
        }

        // 7. POST: Instructor/RemoveMaterial
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMaterial(int courseId, int materialId)
        {
            var trackingRow = await _context.CourseMaterials
                .FirstOrDefaultAsync(cm => cm.CourseId == courseId && cm.MaterialId == materialId);

            if (trackingRow != null)
            {
                _context.CourseMaterials.Remove(trackingRow);

                // Also clean up the orphaned material block
                var material = await _context.Materials.FindAsync(materialId);
                if (material != null) _context.Materials.Remove(material);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ManageSyllabus), new { id = courseId });
        }
    }
}