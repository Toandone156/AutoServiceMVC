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
        private readonly ICommonRepository<Status> _statusRepo;

        public BillController(
            ICommonRepository<Order> orderRepo,
            ICommonRepository<Status> statusRepo
            )
        {
            _orderRepo = orderRepo;
            _statusRepo = statusRepo;
        }

        public async Task<IActionResult> Index(int? id)
        {
            int statusId = id ?? 1;

            var statusRs = await _statusRepo.GetByIdAsync(id);
            var status = statusRs.Data as Status;

            var result = await _orderRepo.GetAllAsync();
            if (result.IsSuccess)
            {
                var data = (result.Data as List<Order>)
                    .Where(o => o.Status.StatusId == statusId)
                    .OrderByDescending(o => o.CreatedAt);

                ViewData["StatusId"] = status.StatusId;
                ViewData["StatusName"] = status.StatusName;

                return View(data);
            }

            TempData["Message"] = "Get data fail";
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> GetBillData(int? id)
        {
            int statusId = id ?? 1;

            var statusRs = await _statusRepo.GetByIdAsync(id);
            var status = statusRs.Data as Status;

            var result = await _orderRepo.GetAllAsync();
            if (result.IsSuccess)
            {
                var data = (result.Data as List<Order>)
                    .Where(o => o.Status.StatusId == statusId)
                    .OrderByDescending(o => o.CreatedAt);

                ViewData["StatusId"] = status.StatusId;
                ViewData["StatusName"] = status.StatusName;
                return PartialView("_BillData", data);
            }

            TempData["Message"] = "Fail to get data";
            return RedirectToAction("Index");
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
