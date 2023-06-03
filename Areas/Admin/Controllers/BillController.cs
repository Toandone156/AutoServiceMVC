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
    public class BillController : Controller
    {
        private readonly ICommonRepository<Order> _orderRepo;

        public BillController(ICommonRepository<Order> orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _orderRepo.GetAllAsync();
            if (result.IsSuccess)
            {
                var data = (result.Data as List<Order>).OrderByDescending(o => o.CreatedAt);
                return View(data);
            }

            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _orderRepo.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                return View(result.Data);
            }

            return RedirectToAction("Index");
        }
    }
}
