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

        // 5. UNIFIED GET: Instructor/EditCourse/{id}
        // This single method now reads all data safely without duplication conflicts
        public async Task<IActionResult> EditCourse(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Instructors)
                .Include(c => c.CourseMaterials)
                    .ThenInclude(cm => cm.Material)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) return NotFound();

            // Pre-loading your lookup data bags for safety
            ViewBag.Domains = new List<string> { ".NET Development", "Machine Learning", "Graph Theory", "Web Development", "Competitive Programming" };
            ViewBag.Instructors = await _context.Instructors.ToListAsync();

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

            var material = new Material { Title = title, YouTubeLink = url };
            _context.Materials.Add(material);
            await _context.SaveChangesAsync();

            var courseMaterial = new CourseMaterial
            {
                CourseId = courseId,
                MaterialId = material.Id,
                Position = position
            };

            _context.CourseMaterials.Add(courseMaterial);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(EditCourse), new { id = courseId });
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

                var material = await _context.Materials.FindAsync(materialId);
                if (material != null) _context.Materials.Remove(material);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(EditCourse), new { id = courseId });
        }

        // 8. POST: Instructor/SaveBulkSyllabus
        [HttpPost]
        public async Task<IActionResult> SaveBulkSyllabus([FromBody] BulkSyllabusSubmissionModel submission)
        {
            if (submission == null || submission.CourseId <= 0)
            {
                return BadRequest("Invalid mapping parameters framework packet.");
            }

            var course = await _context.Courses
                .Include(c => c.CourseMaterials)
                .FirstOrDefaultAsync(c => c.Id == submission.CourseId);

            if (course == null)
            {
                return NotFound("Targeted course validation ID frame missing.");
            }

            if (course.CourseMaterials.Any())
            {
                _context.CourseMaterials.RemoveRange(course.CourseMaterials);
            }

            if (submission.Materials != null && submission.Materials.Any())
            {
                foreach (var incomingItem in submission.Materials)
                {
                    var materialRecord = new Material
                    {
                        Title = incomingItem.Title,
                        YouTubeLink = incomingItem.YouTubeLink
                    };

                    _context.Materials.Add(materialRecord);
                    await _context.SaveChangesAsync();

                    var joinRecord = new CourseMaterial
                    {
                        CourseId = course.Id,
                        MaterialId = materialRecord.Id,
                        Position = incomingItem.Position
                    };

                    _context.CourseMaterials.Add(joinRecord);
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        // 9. POST: Instructor/EditCourse/1 (Saves general info)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourse(int id, Course updatedCourse, int[] selectedInstructors)
        {
            if (id != updatedCourse.Id)
            {
                return BadRequest();
            }

            var courseToUpdate = await _context.Courses
                .Include(c => c.Instructors)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (courseToUpdate == null)
            {
                return NotFound();
            }

            courseToUpdate.Title = updatedCourse.Title;
            courseToUpdate.Domain = updatedCourse.Domain;
            courseToUpdate.Description = updatedCourse.Description;

            courseToUpdate.Instructors.Clear();

            if (selectedInstructors != null)
            {
                foreach (var instId in selectedInstructors)
                {
                    var instructor = await _context.Instructors.FindAsync(instId);
                    if (instructor != null)
                    {
                        courseToUpdate.Instructors.Add(instructor);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }
    }
}