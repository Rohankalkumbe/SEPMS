using Microsoft.AspNetCore.Mvc;
using SEPMS.Data;
using SEPMS.Models;

namespace SEPMS.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            var departments = _context.Departments.ToList();

            return View(departments);
        }

        // Create a CRUD Operation



        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(department);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(department);
        }
    }
}
