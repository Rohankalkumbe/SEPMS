using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEPMS.Application.Abstractions;
using SEPMS.Domain.Entities;

namespace SEPMS.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employees;

        public EmployeesController(IEmployeeService employees)
        {
            _employees = employees;
        }

        public IActionResult Index(string? search, string? status, string? sort, string? dir, int? departmentId, int page = 1, int pageSize = 10)
        {
            var model = _employees.GetPaged(search, status, sort, dir, departmentId, page, pageSize);
            PopulateDepartments(departmentId, forFilter: true);
            return View(model);
        }

        public IActionResult Create()
        {
            PopulateDepartments();
            return View(new Employee
            {
                IsActive = true,
                DateOfJoining = DateTime.Today
            });
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _employees.Create(employee);
                TempData["Success"] = "Employee created.";
                return RedirectToAction("Index");
            }

            PopulateDepartments(employee.DepartmentId);
            return View(employee);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _employees.GetById(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            PopulateDepartments(employee.DepartmentId);
            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (!_employees.Update(employee))
                {
                    return NotFound();
                }

                TempData["Success"] = "Employee updated.";
                return RedirectToAction("Index");
            }

            PopulateDepartments(employee.DepartmentId);
            return View(employee);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _employees.GetByIdWithDepartment(id.Value);
            return employee == null ? NotFound() : View(employee);
        }

        [HttpPost]
        public IActionResult Delete(int employeeId)
        {
            if (!_employees.Delete(employeeId))
            {
                return NotFound();
            }

            TempData["Success"] = "Employee deleted.";
            return RedirectToAction("Index");
        }

        private void PopulateDepartments(int? selectedId = null, bool forFilter = false)
        {
            var list = new SelectList(_employees.GetDepartmentLookup(), "DepartmentId", "DepartmentName", selectedId);
            if (forFilter)
            {
                ViewBag.FilterDepartments = list;
            }
            else
            {
                ViewBag.Departments = list;
            }
        }
    }
}
