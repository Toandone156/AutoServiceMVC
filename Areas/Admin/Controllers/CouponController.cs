using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoServiceMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CouponController : Controller
    {
        private readonly ICommonRepository<Coupon> _couponRepo;

        public CouponController(ICommonRepository<Coupon> couponRepo)
        {
            _couponRepo = couponRepo;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _couponRepo.GetAllAsync();
            if (result.IsSuccess)
            {
                return View(result.Data);
            }

            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _couponRepo.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                return View(result.Data);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("CouponCode,DiscountValue,DiscountPercentage,MinimumOrderAmount,MaximumDiscountAmount," +
            "isForNewUser,Quantity,PointAmount,StartAt,EndAt,CreatorId,UserTypeId")] Coupon coupon)
        {
            //Remove CreatorId => using save user
            var result = await _couponRepo.CreateAsync(coupon);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        // POST: CategoryControler/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind("CouponId,CouponCode,DiscountValue,DiscountPercentage,MinimumOrderAmount,MaximumDiscountAmount," +
            "isForNewUser,Quantity,PointAmount,StartAt,EndAt,CreatorId,UserTypeId")] Coupon coupon)
        {
            var result = await _couponRepo.UpdateAsync(coupon);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            return View("Details", coupon.CouponId);
        }
    }
}
