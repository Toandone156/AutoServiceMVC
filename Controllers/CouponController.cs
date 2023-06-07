using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.Implement;
using AutoServiceMVC.Services.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace AutoServiceMVC.Controllers
{
    [Authorize(AuthenticationSchemes = "User_Scheme")]
    public class CouponController : Controller
    {
        private readonly ICommonRepository<Coupon> _couponRepo;
        private readonly ICommonRepository<UserCoupon> _userCouponRepo;
        private readonly ICommonRepository<User> _userRepo;
        private readonly IPointService _pointService;

        public CouponController(
            ICommonRepository<Coupon> couponRepo,
            ICommonRepository<UserCoupon> userCouponRepo,
            ICommonRepository<User> userRepo,
            IPointService pointService)
        {
            _couponRepo = couponRepo;
            _userCouponRepo = userCouponRepo;
            _userRepo = userRepo;
            _pointService = pointService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = Convert.ToInt32(User.FindFirstValue("Id"));
            var user = (await _userRepo.GetByIdAsync(userId)).Data as User;
            var userCoupons = user.UserCoupons;
            var couponRs = await _couponRepo.GetAllAsync();

            if (couponRs.IsSuccess)
            {
                var coupons = couponRs.Data as IEnumerable<Coupon>;
                var couponsForUser = coupons.Where(c => (
                    (!userCoupons.Any(uc => uc.CouponId == c.CouponId))
                    && ((c.UserTypeId == null) || (c.UserTypeId == 1 && user.UserTypeId == 1) || (c.UserTypeId > 1 && c.UserTypeId <= user.UserTypeId)) 
                    && ((c.EndAt == null) || (c.EndAt >= DateTime.Now))
                    && (c.Remain == null || c.Remain > 0)
                    )
                ).OrderByDescending(c => c.StartAt);

                return View(couponsForUser);
            }

            TempData["Message"] = "Something was wrong";
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> MyCoupon()
        {
            var userId = Convert.ToInt32(User.FindFirstValue("Id"));
            var result = await ((UserCouponRepository)_userCouponRepo).GetByUserIdAsync(userId);

            var userCoupons = result.Data as IEnumerable<UserCoupon>;

            var usefulCoupons = userCoupons.Where(c => (
                        (!c.IsUsed) 
                        && (
                        (c.Coupon.EndAt == null) || (c.Coupon.EndAt >= DateTime.Now)
                    )
                )
            ).OrderByDescending(c => c.Coupon.StartAt);

            return View(usefulCoupons);
        }

        [HttpPost]
        public async Task<JsonResult> CheckCoupon(string couponCode)
        {
            //Check coupon exist
            var couponResult = await ((CouponRepository) _couponRepo).GetByCodeAsync(couponCode);

            if (!couponResult.IsSuccess)
            {
                return Json(new { success = false });
            }

            var coupon = couponResult.Data as Coupon;

            //Check coupon time
            if(coupon.StartAt > DateTime.Now || coupon.EndAt < DateTime.Now)
            {
                return Json(new { success = false });
            }

            //Check coupon for user
            var userId = Convert.ToInt32(User.FindFirstValue("Id"));
            var userCouponRs = await ((UserCouponRepository)_userCouponRepo).GetByUserIdAsync(userId);

            var userCoupons = userCouponRs.Data as List<UserCoupon>;

            if(!userCoupons.Any(x => x.CouponId == coupon.CouponId && x.IsUsed == false))
            {
                return Json(new { success = false });
            }

            string data = JsonConvert.SerializeObject(coupon);

			//return info of coupon
			return Json(new { success = true, data = data }); //Data
        }

        [HttpPost]
        public async Task<JsonResult> TradeCoupon(int id)
        {
            var couponRs = await _couponRepo.GetByIdAsync(id);

            if(!couponRs.IsSuccess)
            {
                return Json(new { success = false, message = "Error to get coupon."});
            }

            var coupon = couponRs.Data as Coupon;

            if(coupon.StartAt > DateTime.Now || coupon.EndAt < DateTime.Now)
            {
                return Json(new { success = false, message = "Coupon is not ready or ended." });
            }

			if (coupon.Remain != null && coupon.Remain < 1)
			{
				return Json(new { success = false, message = "Coupon was not exist." });
			}

			var userId = Convert.ToInt32(User.FindFirstValue("Id"));
            var userRs = await _userRepo.GetByIdAsync(userId);
            var user = userRs.Data as User;

			if (user.UserCoupons.Any(x => x.CouponId == coupon.CouponId))
            {
				return Json(new { success = false, message = "You collected this coupon before." });
			}

            var tradeRs = await _pointService.ChangePointAsync(userId, -coupon.PointAmount, "Trade point to get coupon.");

			if (!tradeRs.IsSuccess)
            {
				return Json(new { success = false, message = tradeRs.Message });
			}

			UserCoupon userCoupon = new UserCoupon()
            {
                CouponId = coupon.CouponId,
                IsUsed = false,
                UserId = userId
            };

            await _userCouponRepo.CreateAsync(userCoupon);

            if(coupon.Remain != null) { 
                coupon.Remain = coupon.Remain - 1;
                await _couponRepo.UpdateAsync(coupon);
            }

			return Json(new { success = true });
        }
    }
}
