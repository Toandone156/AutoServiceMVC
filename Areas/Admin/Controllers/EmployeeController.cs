using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AutoServiceMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Scheme")]
    public class EmployeeController : Controller
    {
        private readonly ICommonRepository<Employee> _employeeRepo;

        public EmployeeController(ICommonRepository<Employee> employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _employeeRepo.GetAllAsync();
            if (result.IsSuccess)
            {
                return View(result.Data);
            }

            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _employeeRepo.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                return View(result.Data);
            }

            return RedirectToAction("Index");
        }

        // POST: CategoryControler/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind("EmployeeId,FullName,Email,RoleId")] Employee employee)
        {
            var result = await _employeeRepo.UpdateAsync(employee);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(String.Empty, result.Message);

            return View("Details", employee.EmployeeId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LayOff(int id)
        {
            var result = await _employeeRepo.GetByIdAsync(id);

            if (result.IsSuccess)
            {
                Employee employee = (Employee) result.Data;
                employee.EndDate = DateTime.Now;

                var updateRs = await _employeeRepo.UpdateAsync(employee);

                if (updateRs.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
            }

            return View("Detail", id);
        }
    }
}
