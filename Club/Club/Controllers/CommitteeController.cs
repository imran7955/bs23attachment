using Microsoft.AspNetCore.Mvc;
using Club.Data;
using Club.Models;
using System.Linq;

namespace Club.Controllers
{
    public class CommitteeController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Visual Studio automatically passes your DbContext here via Dependency Injection
        public CommitteeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: /Committee/Index (Shows the list of members)
        public IActionResult Index()
        {
            // Fetch all members from the database table and convert to a list
            var members = _context.CommitteeMembers.ToList();
            return View(members);
        }

        // 2. GET: /Committee/Add (Shows the empty HTML form)
        public IActionResult Add()
        {
            return View();
        }

        // 3. POST: /Committee/Add (Handles the form submission when saving a member)
        [HttpPost]
        [ValidateAntiForgeryToken] // Protects against Cross-Site Request Forgery (CSRF) attacks
        public IActionResult Add(CommitteeMember member)
        {
            if (ModelState.IsValid)
            {
                // Add the new member object to the EF Core tracking context
                _context.CommitteeMembers.Add(member);

                // Save the changes into your SQL Server database
                _context.SaveChanges();

                // Redirect back to the list page
                return RedirectToAction(nameof(Index));
            }

            // If the data was invalid, redisplay the form with the errors
            return View(member);
        }
    }
}