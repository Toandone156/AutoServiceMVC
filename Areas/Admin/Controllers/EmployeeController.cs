using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

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
            var EmplId = Convert.ToInt32(User.FindFirstValue("Id"));
            var result = await _employeeRepo.GetAllAsync();
            if (result.IsSuccess)
            {
                var emplList = (result.Data as List<Employee>).Where(e => e.EmployeeId != EmplId);
                return View(emplList);
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
            [Bind("EmployeeId,FullName,RoleId")] Employee employee)
        {
            var emplRs = await _employeeRepo.GetByIdAsync(employee.EmployeeId);
            if (emplRs.IsSuccess)
            {
                var updateEmpl = emplRs.Data as Employee;

                updateEmpl.FullName = employee.FullName;
                updateEmpl.RoleId = employee.RoleId;

                await _employeeRepo.UpdateAsync(updateEmpl);

				TempData["Message"] = "Update successful";
				return RedirectToAction("Index");
            }

            TempData["Message"] = emplRs.Message;

            return View("Details", employee.EmployeeId);
        }

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
