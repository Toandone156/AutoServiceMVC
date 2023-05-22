using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoServiceMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly IAuthenticateService<Employee> _emplService;
        private readonly ICommonRepository<Role> _roleRepo;

        public AuthController(IAuthenticateService<Employee> emplService, ICommonRepository<Role> roleRepo)
        {
            _emplService = emplService;
            _roleRepo = roleRepo;
        }

        public ActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Register()
        {
            var status = await _roleRepo.GetAllAsync();
            ViewBag.RoleList = status.Data;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username,Password")] Login login)
        {
            if (ModelState.IsValid)
            {
                var status = await _emplService.ValidateLoginAsync(login);

                if (status.IsSuccess)
                {
                    return RedirectToAction("Index", "Home", new {area = "Admin"});
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username,Password,AgainPassword,FullName,Email,RoleId")]
                                Register register)
        {
            if (ModelState.IsValid)
            {
                var status = await _emplService.RegisterAsync(register);

                if (status.IsSuccess)
                {
                    return RedirectToAction("Index", "Employee", new {area = "Admin"});
                }
            }

            return RedirectToAction("Register");
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}
