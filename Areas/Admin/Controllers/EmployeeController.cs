using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoServiceMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly ICommonRepository<Employee> _employeeRepo;
        private readonly ICommonRepository<Role> _roleRepo;

        public EmployeeController(ICommonRepository<Employee> employeeRepo, ICommonRepository<Role> roleRepo)
        {
            _employeeRepo = employeeRepo;
            _roleRepo = roleRepo;
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
                var roleResult = await _roleRepo.GetAllAsync();
                ViewBag.RoleList = roleResult.Data;
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

            return RedirectToAction("Details", employee.EmployeeId);
        }

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

            return RedirectToAction("Detail", new {id = id });
        }
    }
}
