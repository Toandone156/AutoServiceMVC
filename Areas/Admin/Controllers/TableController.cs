using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AutoServiceMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "Admin_Scheme")]
    public class TableController : Controller
    {
        private readonly ICommonRepository<Table> _tableRepo;

        public TableController(ICommonRepository<Table> tableRepo)
        {
            _tableRepo = tableRepo;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _tableRepo.GetAllAsync();
            if (result.IsSuccess)
            {
                return View(result.Data);
            }

            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _tableRepo.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                return View(result.Data);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Scheme")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Scheme")]
        public async Task<IActionResult> Create(
            [Bind("TableName,TableCode")] Table table)
        {
            var result = await _tableRepo.CreateAsync(table);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        // POST: CategoryControler/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Scheme")]
        public async Task<IActionResult> Edit(
            [Bind("TableId,TableName,TableCode")] Table table)
        {
            var result = await _tableRepo.UpdateAsync(table);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            return View("Details", table.TableId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Scheme")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _tableRepo.DeleteByIdAsync(id);

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
