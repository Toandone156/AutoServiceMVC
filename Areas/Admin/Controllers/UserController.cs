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
    public class UserController : Controller
    {
        private readonly ICommonRepository<User> _userRepo;

        public UserController(ICommonRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _userRepo.GetAllAsync();
            if (result.IsSuccess)
            {
                var users = (result.Data as List<User>).OrderByDescending(u => u.UserId);
                return View(users);
            }

            TempData["Message"] = "Get data fail";
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _userRepo.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                return View(result.Data);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ban(int id)
        {
            return View("Index");
        }
    }
}
