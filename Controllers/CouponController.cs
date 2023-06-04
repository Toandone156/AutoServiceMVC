using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.Implement;
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

        public CouponController(
            ICommonRepository<Coupon> couponRepo,
            ICommonRepository<UserCoupon> userCouponRepo,
            ICommonRepository<User> userRepo)
        {
            _couponRepo = couponRepo;
            _userCouponRepo = userCouponRepo;
            _userRepo = userRepo;
        }

        public async Task<IActionResult> Index()
        {
            var userId = Convert.ToInt32(User.FindFirstValue("Id"));
            var userCouponRs = await _userCouponRepo.GetAllAsync();
            var couponRs = await _couponRepo.GetAllAsync();

            if (couponRs.IsSuccess && userCouponRs.IsSuccess)
            {
                var coupons = couponRs.Data as IEnumerable<Coupon>;
                var userCoupons = userCouponRs.Data as IEnumerable<UserCoupon>;
                var couponsForUser = coupons.Where(c => (
                    (!userCoupons.Any(uc => uc.CouponId == c.CouponId))
                    && ((c.UserTypeId == null) || (c.UserTypeId <= userId)) 
                    && ((c.EndAt == null) || (c.EndAt >= DateTime.Now))
                    && (c.Quantity > 0)
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
                return Json(new {success = false});
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

            if(!userCoupons.Any(x => x.CouponId == coupon.CouponId))
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

			if (coupon.Remain < 1)
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

			if (user.Point < coupon.PointAmount)
            {
				return Json(new { success = false, message = "Your point is not enough." });
			}

			UserCoupon userCoupon = new UserCoupon()
            {
                CouponId = coupon.CouponId,
                IsUsed = false,
                UserId = userId
            };

            await _userCouponRepo.CreateAsync(userCoupon);

            coupon.Remain--;
            await _couponRepo.UpdateAsync(coupon);

			return Json(new { success = true });
        }
    }
}
