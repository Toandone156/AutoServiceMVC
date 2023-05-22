using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoServiceMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("TableName,TableCode")] Table table)
        {
            var result = await _tableRepo.CreateAsync(table);
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
            [Bind("TableId,TableName,TableCode")] Table table)
        {
            var result = await _tableRepo.UpdateAsync(table);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Details", table.TableId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
