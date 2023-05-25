using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoServiceMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "Admin_Scheme")]
    public class CategoryController : Controller
    {
        private readonly ICommonRepository<Category> _categoryRepo;

        public CategoryController(ICommonRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _categoryRepo.GetAllAsync();
            if (result.IsSuccess)
            {
                ViewBag.Message = TempData["Message"];
                return View(result.Data);
            }

            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _categoryRepo.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                return View(result.Data);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin",AuthenticationSchemes = "Admin_Scheme")]
        public async Task<IActionResult> Create(
            [Bind("CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryRepo.CreateAsync(category);
                if (result.IsSuccess)
                {
                    TempData["Message"] = "Create Category success";
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(String.Empty, result.Message);
            }

            return View();
        }

        // POST: CategoryControler/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Scheme")]
        public async Task<IActionResult> Edit(
            [Bind("CategoryId,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryRepo.UpdateAsync(category);
                if (result.IsSuccess)
                {
                    TempData["Message"] = "Edit Category success";
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(String.Empty, result.Message);
            }

            return View("Details", category.CategoryId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Scheme")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryRepo.DeleteByIdAsync(id);
            TempData["Message"] = "Delete Category success";
            return RedirectToAction("Index");
        }
    }
}
