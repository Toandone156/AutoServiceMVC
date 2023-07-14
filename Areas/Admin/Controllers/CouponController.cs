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
                var coupons = (result.Data as List<Coupon>)
                    .OrderByDescending(c => DateTime.Compare(c.EndAt ?? DateTime.Now, DateTime.Now))
                    .ThenBy(c => c.EndAt);

                return View(coupons);
            }

            TempData["Message"] = "Get data fail";
            return View();
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Scheme")]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _couponRepo.GetByIdAsync(id);
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
            [Bind("CouponCode,DiscountValue,DiscountPercentage,MinimumOrderAmount,MaximumDiscountAmount," +
            "Quantity,PointAmount,StartAt,EndAt,CreatorId,UserTypeId")] Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                var result = await _couponRepo.CreateAsync(coupon);
                if (result.IsSuccess)
                {
                    TempData["Message"] = "Create coupon success";
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

        // POST: CategoryControler/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Scheme")]
        public async Task<IActionResult> Edit(
            [Bind("CouponId,CouponCode,DiscountValue,DiscountPercentage,MinimumOrderAmount,MaximumDiscountAmount," +
            "Quantity,PointAmount,StartAt,EndAt,CreatorId,UserTypeId")] Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                var preCoupon = (await _couponRepo.GetByIdAsync(coupon.CouponId)).Data as Coupon;

                if ((preCoupon.Remain + (coupon.Quantity - preCoupon.Quantity)) < 0)
                {
					ModelState.AddModelError(String.Empty, "Remain is not enough to decrease");
					return RedirectToAction("Details", new {id = coupon.CouponId });
				}

                coupon.Remain = preCoupon.Remain + (coupon.Quantity - preCoupon.Quantity);

                var result = await _couponRepo.UpdateAsync(coupon);
                if (result.IsSuccess)
                {
                    return RedirectToAction("Index");
                }

                TempData["Message"] = result.Message;
            }
            else
            {
                TempData["Message"] = "Some fields is invalid";
            }

            return RedirectToAction("Details", coupon.CouponId);
        }

        public async Task<IActionResult> Stop(int id)
        {
            var couponRs = await _couponRepo.GetByIdAsync(id);
            if(couponRs.IsSuccess)
            {
                var coupon = couponRs.Data as Coupon;
                coupon.EndAt = DateTime.Now;
                await _couponRepo.UpdateAsync(coupon);

                TempData["Message"] = "Coupon was ended success";
                return RedirectToAction("Index");
            }

            TempData["Message"] = couponRs.Message;
            return RedirectToAction("Details", id);
        }
    }
}
