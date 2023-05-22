using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoServiceMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
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
        public async Task<IActionResult> Create(
            [Bind("CategoryName")] Category category)
        {
            var result = await _categoryRepo.CreateAsync(category);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Create");
        }

        // POST: CategoryControler/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind("CategoryId,CategoryName")] Category category)
        {
            var result = await _categoryRepo.UpdateAsync(category);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Details", category.CategoryId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _categoryRepo.DeleteByIdAsync(id);

                if (result.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
            }catch(Exception ex)
            {

            }

            return RedirectToAction("Index");
        }
    }
}
