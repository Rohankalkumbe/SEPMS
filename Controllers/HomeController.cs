using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SEPMS.Application.Abstractions;
using SEPMS.Models;

namespace SEPMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;

        public HomeController(
            IEmployeeService employeeService,
            IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel
            {
                TotalEmployees = await _employeeService.GetTotalCountAsync(),

                TotalDepartments = await _departmentService.GetTotalCountAsync(),

                ActiveEmployees = await _employeeService.GetActiveCountAsync(),

                InactiveEmployees = await _employeeService.GetInactiveCountAsync()
            };

            ViewData["Title"] = "Dashboard";
            ViewData["PageTheme"] = "org";
            ViewData["CurrentNav"] = "home";

            return View(model);
        }

        public IActionResult Privacy()
        {
            return RedirectToAction("Index", "Privacy");
        }
    }
}
