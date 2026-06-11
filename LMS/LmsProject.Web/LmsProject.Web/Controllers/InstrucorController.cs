using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LmsProject.Application.Services;
using LmsProject.Domain.Entities;
using LmsProject.Web.Models;

namespace LmsProject.Web.Controllers
{
    public class InstructorController : Controller
    {
        private readonly IInstructorService _instructorService;
        private readonly ICourseService _courseService;

        public InstructorController(IInstructorService instructorService, ICourseService courseService)
        {
            _instructorService = instructorService;
            _courseService = courseService;
        }

        // GET: Instructor/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var courses = await _courseService.GetFilteredCoursesAsync("All", "All", "");
            var instructors = await _instructorService.GetAllInstructorsAsync();

            var model = new CourseViewModel
            {
                Courses = courses.ToList(),
                Instructors = instructors.ToList(),
                Domains = (await _courseService.GetFilterDomainsAsync()).ToList(),
                SelectedDomain = "All",
                SelectedInstructor = "All",
                SearchTerm = string.Empty
            };

            return View(model);
        }

        // GET: Instructor/CreateInstructor
        public IActionResult CreateInstructor()
        {
            return View();
        }

        // POST: Instructor/CreateInstructor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInstructor(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                await _instructorService.RegisterInstructorAsync(name);
                return RedirectToAction(nameof(Dashboard));
            }
            return View();
        }

        // GET: Instructor/CreateCourse
        public async Task<IActionResult> CreateCourse()
        {
            var domainsList = await _courseService.GetFilterDomainsAsync();
            var allInstructorsList = await _instructorService.GetAllInstructorsAsync();

            ViewBag.Domains = domainsList.ToList();
            ViewBag.Instructors = allInstructorsList.ToList();

            return View();
        }

        // POST: Instructor/CreateCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(string title, string domain, string description, List<int> selectedInstructors)
        {
            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(domain))
            {
                var instructorIds = selectedInstructors ?? new List<int>();
                await _instructorService.RegisterCourseAsync(title, domain, description, instructorIds);
                return RedirectToAction(nameof(Dashboard));
            }

            ViewBag.Domains = (await _courseService.GetFilterDomainsAsync()).ToList();
            ViewBag.Instructors = (await _instructorService.GetAllInstructorsAsync()).ToList();

            return View();
        }

        // GET: Instructor/EditCourse/5
        public async Task<IActionResult> EditCourse(int id)
        {
            var course = await _courseService.GetCourseDetailsAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            ViewBag.Domains = await _courseService.GetFilterDomainsAsync();
            ViewBag.Instructors = await _instructorService.GetAllInstructorsAsync();
            return View(course);
        }

        // POST: Instructor/EditCourse/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourse(int id, string title, string domain, string description, List<int> selectedInstructors)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid course tracking reference identifier context.");
            }

            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(domain))
            {
                var instructorIds = selectedInstructors ?? new List<int>();

                // CORESWAP FIX: Invoke the accurate updates layer endpoint instead of Register insertion routines
                await _instructorService.UpdateCourseDetailsAsync(id, title, domain, description, instructorIds);

                return RedirectToAction(nameof(Dashboard));
            }

            ViewBag.Domains = await _courseService.GetFilterDomainsAsync();
            ViewBag.Instructors = await _instructorService.GetAllInstructorsAsync();

            var originalCourseFallbackModel = await _courseService.GetCourseDetailsAsync(id);
            return View(originalCourseFallbackModel);
        }

        // POST: Instructor/SaveBulkSyllabus
        [HttpPost]
        public async Task<IActionResult> SaveBulkSyllabus([FromBody] WebBulkSyllabusRequest request)
        {
            if (request == null || request.CourseId <= 0)
            {
                return BadRequest("Invalid payload format mapping request.");
            }

            var applicationMaterialsList = request.Materials.Select(m => new LmsProject.Application.DTOs.SyllabusItemDto
            {
                Title = m.Title,
                YouTubeLink = m.YouTubeLink,
                Position = m.Position
            }).ToList();

            await _instructorService.SaveBulkSyllabusAsync(request.CourseId, applicationMaterialsList);
            return Ok();
        }
    }

    public class WebBulkSyllabusRequest
    {
        public int CourseId { get; set; }
        public List<WebSyllabusItem> Materials { get; set; } = new List<WebSyllabusItem>();
    }

    public class WebSyllabusItem
    {
        public string Title { get; set; } = string.Empty;
        public string YouTubeLink { get; set; } = string.Empty;
        public int Position { get; set; }
    }
}