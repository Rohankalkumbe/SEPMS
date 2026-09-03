using Microsoft.AspNetCore.Mvc;
using SEPMS.Application.Abstractions;
using SEPMS.Domain.Entities;

namespace SEPMS.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departments;

        public DepartmentController(IDepartmentService departments)
        {
            _departments = departments;
        }

        public IActionResult Index(string? search, string? status, string? sort, string? dir, int page = 1, int pageSize = 10)
        {
            return View(_departments.GetPaged(search, status, sort, dir, page, pageSize));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _departments.Create(department);
                TempData["Success"] = "Department created.";
                return RedirectToAction("Index");
            }

            return View(department);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = _departments.GetById(id.Value);
            return department == null ? NotFound() : View(department);
        }

        [HttpPost]
        public IActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                if (!_departments.Update(department))
                {
                    return NotFound();
                }

                TempData["Success"] = "Department updated.";
                return RedirectToAction("Index");
            }

            return View(department);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = _departments.GetById(id.Value);
            return department == null ? NotFound() : View(department);
        }

        [HttpPost]
        public IActionResult Delete(int departmentId)
        {
            if (!_departments.Delete(departmentId))
            {
                return NotFound();
            }

            TempData["Success"] = "Department deleted.";
            return RedirectToAction("Index");
        }
    }
}
