using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoServiceMVC.Data;
using AutoServiceMVC.Services;
using AutoServiceMVC.Models;

namespace AutoServiceMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICommonRepository<Category> _category;

        public CategoryController(AppDbContext context, ICommonRepository<Category> category)
        {
            _context = context;
            _category = category;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            var result = await _category.GetAllAsync();

            if (result.IsSuccess)
            {
                return View(result.Data);
            }
            else
            {
                return View();
            }
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var result = await _category.GetByIdAsync(id);

            if (result.IsSuccess)
            {
                return View(result.Data);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                var result = await _category.CreateAsync(category);
                if(result.IsSuccess) return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Some fields is invalid");
            }

            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var result = await _category.GetByIdAsync(id);

            if (result.IsSuccess)
            {
                return View(result.Data);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _category.UpdateAsync(category);

                if (result.IsSuccess) return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var result = await _category.GetByIdAsync(id);

            if (result.IsSuccess)
            {
                return View(result.Data);
            }

            return NotFound();
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Category entity)
        {
            var result = await _category.DeleteByIdAsync(entity.CategoryId);
            if(result.IsSuccess) 
            { 
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
