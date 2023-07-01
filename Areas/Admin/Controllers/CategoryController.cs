using AutoServiceMVC.Migrations;
using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using System;

namespace AutoServiceMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin",AuthenticationSchemes = "Admin_Scheme")]
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

            TempData["Message"] = "Get data fail";
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
            if (ModelState.IsValid)
            {
                var result = await _categoryRepo.CreateAsync(category);
                if (result.IsSuccess)
                {
                    TempData["Message"] = "Create category success";
                    return RedirectToAction("Index");
                }

                TempData["Message"] = result.Message;
            }
            else
            {
                TempData["Message"] = "Some fields is invalid";
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind("CategoryId,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryRepo.UpdateAsync(category);
                if (result.IsSuccess)
                {
                    TempData["Message"] = "Update success";
                    return RedirectToAction("Index");
                }

                TempData["Message"] = result.Message;
            }
            else
            {
                TempData["Message"] = "Some fields is invalid";
            }

            return View("Details", category.CategoryId);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryRepo.DeleteByIdAsync(id);

            if(result.IsSuccess)
            {
                TempData["Message"] = "Delete category success";
                return RedirectToAction("Index");
            }

            TempData["Message"] = result.Message;
            return RedirectToAction("Index");
        }
    }
}
