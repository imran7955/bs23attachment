using Microsoft.AspNetCore.Mvc;
using MvcFlowDemo.Models;
using System.Collections.Generic;

namespace MvcFlowDemo.Controllers
{
    public class StudentController : Controller
    {
        private static List<Student> _students = new List<Student>();

        public IActionResult About()
        {
            return View(_students);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            _students.Add(student);
            return RedirectToAction("About");
        }
    }
}